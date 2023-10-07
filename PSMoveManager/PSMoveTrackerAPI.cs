/**
* PS Move API - An interface for the PS Move Motion Controller
* Copyright (c) 2012 Benjamin Venditti <benjamin.venditti@gmail.com>
* Copyright (c) 2012 Thomas Perl <m@thp.io>
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*    1. Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*
*    2. Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
* LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
* CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
* CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
* ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
* POSSIBILITY OF SUCH DAMAGE.
**/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PSMove
{
    public struct PSMoveTrackerRGBImage
    {
        IntPtr data;
        int width;
        int height;
    } /*!< Structure for storing RGB image data */

    /*! Status of the tracker */
    public enum PSMoveTrackerStatus
    {
        NotCalibrated, /*!< Controller not registered with tracker */
        CalibrationError, /*!< Calibration failed (check lighting, visibility) */
        Calibrated, /*!< Color calibration successful, not currently tracking */
        Tracking, /*!< Calibrated and successfully tracked in the camera */
    };

    /*! Exposure modes */
    public enum PSMoveTrackerExposure
    {
        Low, /*!< Very low exposure: Good tracking, no environment visible */
        Medium, /*!< Middle ground: Good tracking, environment visibile */
        High, /*!< High exposure: Fair tracking, but good environment */
        Invalid, /*!< Invalid exposure value (for returning failures) */
    };

    /* A structure to retain the tracker settings. Typically these do not change after init & calib.*/
    public struct PSMoveTrackerSettings
    {

        /* Camera Controls*/
        int camera_frame_width;                     /* [0=auto] */
        int camera_frame_height;                    /* [0=auto] */
        int camera_frame_rate;                      /* [0=auto] */
        PSMoveBool camera_auto_gain;          /* [PSMove_False] */
        int camera_gain;                            /* [0] [0,0xFFFF] */
        PSMoveBool camera_auto_white_balance; /* [PSMove_False] */
        int camera_exposure;                        /* [(255 * 15) / 0xFFFF] [0,0xFFFF] */
        int camera_brightness;                      /* [0] [0,0xFFFF] */
        PSMoveBool camera_mirror;             /* [PSMove_True] mirror camera image horizontally */

        /* Settings for camera calibration process */
        PSMoveTrackerExposure exposure_mode;  /* [Exposure_LOW] exposure mode for setting target luminance */
        int calibration_blink_delay;                /* [200] number of milliseconds to wait between a blink  */
        int calibration_diff_t;                     /* [20] during calibration, all grey values in the diff image below this value are set to black  */
        int calibration_min_size;                   /* [50] minimum size of the estimated glowing sphere during calibration process (in pixel)  */
        int calibration_max_distance;               /* [30] maximum displacement of the separate found blobs  */
        int calibration_size_std;                   /* [10] maximum standard deviation (in %) of the glowing spheres found during calibration process  */
        int color_mapping_max_age;                  /* [2*60*60] Only re-use color mappings "younger" than this time in seconds  */
        float dimming_factor;                       /* [1.f] dimming factor used on LED RGB values  */

        /* Settings for OpenCV image processing for sphere detection */
        int color_hue_filter_range;                 /* [20] +- range of Hue window of the hsv-colorfilter  */
        int color_saturation_filter_range;          /* [85] +- range of Sat window of the hsv-colorfilter  */
        int color_value_filter_range;               /* [85] +- range of Value window of the hsv-colorfilter  */

        /* Settings for tracker algorithms */
        int tracker_adaptive_xy;                    /* [1] specifies to use a adaptive x/y smoothing  */
        int tracker_adaptive_z;                     /* [1] specifies to use a adaptive z smoothing  */
        float color_adaption_quality_t;             /* [35] maximal distance (calculated by 'psmove_tracker_hsvcolor_diff') between the first estimated color and the newly estimated  */
        float color_update_rate;                    /* [1] every x seconds adapt to the color, 0 means no adaption  */
        // size of "search" tiles when tracking is lost
        int search_tile_width;                      /* [0=auto] width of a single tile */
        int search_tile_height;                     /* height of a single tile */
        int search_tiles_horizontal;                /* number of search tiles per row */
        int search_tiles_count;                     /* number of search tiles */

        /* THP-specific tracker threshold checks */
        int roi_adjust_fps_t;                       /* [160] the minimum fps to be reached, if a better roi-center adjusment is to be perfomred */
        // if tracker thresholds not met, sphere is deemed not to be found
        float tracker_quality_t1;                   /* [0.3f] minimum ratio of number of pixels in blob vs pixel of estimated circle. */
        float tracker_quality_t2;                   /* [0.7f] maximum allowed change of the radius in percent, compared to the last estimated radius */
        float tracker_quality_t3;                   /* [4.7f] minimum radius  */
        // if color thresholds not met, color is not adapted
        float color_update_quality_t1;              /* [0.8] minimum ratio of number of pixels in blob vs pixel of estimated circle. */
        float color_update_quality_t2;              /* [0.2] maximum allowed change of the radius in percent, compared to the last estimated radius */
        float color_update_quality_t3;              /* [6.f] minimum radius */

        /* Camera calibration */
        string intrinsics_xml;                  /* [intrinsics.xml] File to read intrinsics matrix from */
        string distortion_xml;                  /* [distortion.xml] File to read distortion coefficients from */
    } /*!< Structure for storing tracker settings */

    public class PSMoveTrackerAPI
    {
        /* Defines the range of x/y values for the position getting, etc.. */
        public const int PSMOVE_TRACKER_DEFAULT_WIDTH = 640;
        public const int PSMOVE_TRACKER_DEFAULT_HEIGHT = 480;
        public const int PSMOVE_TRACKER_DEFAULT_FPS = 60;

        /* Maximum number of controllers that can be tracked at once */
        public const int PSMOVE_TRACKER_MAX_CONTROLLERS = 5;

        /* Name of the environment variable used to pick a camera */
        public const string PSMOVE_TRACKER_CAMERA_ENV = "PSMOVE_TRACKER_CAMERA";

        /* Name of the environment variable used to choose a pre-recorded video */
        public const string PSMOVE_TRACKER_FILENAME_ENV = "PSMOVE_TRACKER_FILENAME";

        /* Name of the environment variable used to set the biggest ROI size */
        public const string PSMOVE_TRACKER_ROI_SIZE_ENV = "PSMOVE_TRACKER_ROI_SIZE";

        /* Name of the environment variable used to set a fixed tracker color */
        public const string PSMOVE_TRACKER_COLOR_ENV = "PSMOVE_TRACKER_COLOR";

        /* Name of the environment variables for the camera image size */
        public const string PSMOVE_TRACKER_WIDTH_ENV = "PSMOVE_TRACKER_WIDTH";
        public const string PSMOVE_TRACKER_HEIGHT_ENV = "PSMOVE_TRACKER_HEIGHT";

        /* Field of view of the PS Eye */
        public const int PSEYE_FOV_BLUE_DOT = 75;
        public const int PSEYE_FOV_RED_DOT = 56;

        /**
        * \brief Initializes a tracker settings with default values
        *
        * If you want to initialize a camera with custom settings,
        * it's recommended you initialize the settings struct with this first
        *
        **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_settings_set_default(ref PSMoveTrackerSettings settings);

        /**
         * \brief Create a new PS Move Tracker instance and open the camera
         *
         * This will select the best camera for tracking (this means that if
         * a PSEye is found, it will be used, otherwise the first available
         * camera will be used as fallback).
         *
         * Uses default settings
         *
         * \return A new \ref PSMoveTracker instance or \c NULL on error
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_tracker_new();

        /**
        * \brief Create a new PS Move Tracker instance and open the camera
        *
        * This will select the best camera for tracking (this means that if
        * a PSEye is found, it will be used, otherwise the first available
        * camera will be used as fallback).
        *
        * \param settings Tracker and camera settings to use
        *
        * \return A new \ref PSMoveTracker instance or \c NULL on error
        **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_tracker_new_with_settings(ref PSMoveTrackerSettings settings);

        /**
         * \brief Create a new PS Move Tracker instance with a specific camera
         *
         * This function can be used when multiple cameras are available to
         * force the use of a specific camera.
         *
         * Usually it's better to use psmove_tracker_new() and let the library
         * choose the best camera, unless you have a good reason not to.
         *
         * \param camera Zero-based index of the camera to use
         *
         * \return A new \ref PSMoveTracker instance or \c NULL on error
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_tracker_new_with_camera(int camera);

        /**
        * \brief Create a new PS Move Tracker instance with a specific camera
        *
        * This function can be used when multiple cameras are available to
        * force the use of a specific camera.
        *
        * Usually it's better to use psmove_tracker_new() and let the library
        * choose the best camera, unless you have a good reason not to.
        *
        * \param camera Zero-based index of the camera to use
        * \param settings Tracker and camera settings to use
        *
        * \return A new \ref PSMoveTracker instance or \c NULL on error
        **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_tracker_new_with_camera_and_settings(int camera, ref PSMoveTrackerSettings settings);

        /**
        * \brief Get the number of available cameras
        *
        * \return Number of cameras available, or -1 if unknown
        **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_count_connected();

        /**
         * \brief Configure if the LEDs of a controller should be auto-updated
         *
         * If auto-update is enabled (the default), the tracker will set and
         * update the LEDs of the controller automatically. If not, the user
         * must set the LEDs of the controller and update them regularly. In
         * that case, the user can use psmove_tracker_get_color() to determine
         * the color that the controller's LEDs have to be set to.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         * \param auto_update_leds \ref PSMove_True to auto-update LEDs from
         *                         the tracker, \ref PSMove_False if the user
         *                         will take care of updating the LEDs
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_set_auto_update_leds(IntPtr tracker, IntPtr move, PSMoveBool auto_update_leds);

        /**
         * \brief Check if the LEDs of a controller are updated automatically
         *
         * This is the getter function for psmove_tracker_set_auto_update_leds().
         * See there for details on what auto-updating LEDs means.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         *
         * \return \ref PSMove_True if the controller's LEDs are set to be
         *         updated automatically, \ref PSMove_False otherwise
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveBool psmove_tracker_get_auto_update_leds(IntPtr tracker, IntPtr move);

        /**
         * \brief Set the LED dimming value for all controller
         *
         * Usually it's not necessary to call this function, as the dimming
         * is automatically determined when the first controller is enabled.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param dimming A value in the range from 0 (LEDs switched off) to
         *                1 (full LED intensity)
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_set_dimming(IntPtr tracker, float dimming);

        /**
         * \brief Get the LED dimming value for all controllers
         *
         * See psmove_tracker_set_dimming() for details.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         *
         * \return The dimming value for the LEDs
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static float psmove_tracker_get_dimming(IntPtr tracker);

        /**
         * \brief Set the desired camera exposure mode
         *
         * This function sets the desired exposure mode. This should be
         * called before controllers are added to the tracker, so that the
         * dimming for the controllers can be determined for the specific
         * exposure setting.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param exposure One of the \ref PSMoveTracker_Exposure values
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_set_exposure(IntPtr tracker, PSMoveTrackerExposure exposure);

        /**
         * \brief Get the desired camera exposure mode
         *
         * See psmove_tracker_set_exposure() for details.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         *
         * \return One of the \ref PSMoveTracker_Exposure values
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveTrackerExposure psmove_tracker_get_exposure(IntPtr tracker);

        /**
         * \brief Enable or disable camera image deinterlacing (line doubling)
         *
         * Enables or disables camera image deinterlacing for this tracker.
         * You usually only want to enable deinterlacing if your camera source
         * provides interlaced images (e.g. 1080i). The interlacing will be
         * removed by doubling every other line. By default, deinterlacing is
         * disabled.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param enabled \ref PSMove_True to enable deinterlacing,
         *                \ref PSMove_False to disable deinterlacing (default)
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_enable_deinterlace(IntPtr tracker, PSMoveBool enabled);

        /**
         * \brief Enable or disable horizontal camera image mirroring
         *
         * Enables or disables horizontal mirroring of the camera image. The
         * mirroring setting will affect the X coordinates of the controller
         * positions tracked, as well as the output image. In addition, the
         * sensor fusion module will mirror the orientation information if
         * mirroring is set here. By default, mirroring is disabled.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param enabled \ref PSMove_True to mirror the image horizontally,
         *                \ref PSMove_False to leave the image as-is (default)
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_set_mirror(IntPtr tracker, PSMoveBool enabled);

        /**
         * \brief Query the current camera image mirroring state
         *
         * See psmove_tracker_set_mirror() for details.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         *
         * \return \ref PSMove_True if mirroring is enabled,
         *         \ref PSMove_False if mirroring is disabled
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveBool psmove_tracker_get_mirror(IntPtr tracker);

        /**
         * \brief Enable tracking of a motion controller
         *
         * Calling this function will register the controller with the
         * tracker, and start blinking calibration. The user should hold
         * the motion controller in front of the camera and wait for the
         * calibration to finish.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         *
         * \return \ref Tracker_CALIBRATED if calibration succeeded
         * \return \ref Tracker_CALIBRATION_ERROR if calibration failed
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveTrackerStatus psmove_tracker_enable(IntPtr tracker, IntPtr move);

        /**
         * \brief Enable tracking with a custom sphere color
         *
         * This function does basically the same as psmove_tracker_enable(),
         * but forces the sphere color to a pre-determined value.
         *
         * Using this function might give worse tracking results, because
         * the color might not be optimal for a given lighting condition.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         * \param r The red intensity of the desired color (0..255)
         * \param g The green intensity of the desired color (0..255)
         * \param b The blue intensity of the desired color (0..255)
         *
         * \return \ref Tracker_CALIBRATED if calibration succeeded
         * \return \ref Tracker_CALIBRATION_ERROR if calibration failed
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveTrackerStatus psmove_tracker_enable_with_color(IntPtr tracker, IntPtr move, byte r, byte g, byte b);

        /**
         * \brief Disable tracking of a motion controller
         *
         * If the \ref PSMove instance was never enabled, this function
         * does nothing. Otherwise it removes the instance from the
         * tracker and stops tracking the controller.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_disable(IntPtr tracker, IntPtr move);

        /**
         * \brief Get the desired sphere color of a motion controller
         *
         * Get the sphere color of the controller as it is set using
         * psmove_update_leds(). This is not the color as the sphere
         * appears in the camera - for that, see
         * psmove_tracker_get_camera_color().
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A Valid \ref PSmove handle
         * \param r Pointer to store the red component of the color
         * \param g Pointer to store the green component of the color
         * \param g Pointer to store the blue component of the color
         *
         * \return Nonzero if the color was successfully returned, zero if
         *         if the controller is not enabled of calibration has not
         *         completed yet.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_get_color(IntPtr tracker, IntPtr move, ref byte r, ref byte g, ref byte b);

        /**
         * \brief Get the sphere color of a controller in the camera image
         *
         * Get the sphere color of the controller as it currently
         * appears in the camera image. This is not the color that is
         * set using psmove_update_leds() - for that, see
         * psmove_tracker_get_color().
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A Valid \ref PSmove handle
         * \param r Pointer to store the red component of the color
         * \param g Pointer to store the green component of the color
         * \param g Pointer to store the blue component of the color
         *
         * \return Nonzero if the color was successfully returned, zero if
         *         if the controller is not enabled of calibration has not
         *         completed yet.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_get_camera_color(IntPtr tracker, IntPtr move, ref byte r, ref byte g, ref byte b);

        /**
         * \brief Set the sphere color of a controller in the camera image
         *
         * This function should only be used in special situations - it is
         * usually not required to manually set the sphere color as it appears
         * in the camera image, as this color is determined at runtime during
         * blinking calibration. For some use cases, it might be useful to
         * set the color manually (e.g. when the user should be able to select
         * the color in the camera image after lighting changes).
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         * \param r The red component of the color (0..255)
         * \param g The green component of the color (0..255)
         * \param b The blue component of the color (0..255)
         *
         * \return Nonzero if the color was successfully set, zero if
         *         if the controller is not enabled of calibration has not
         *         completed yet.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_set_camera_color(IntPtr tracker, IntPtr move, byte r, byte g, byte b);

        /**
         * \brief Query the tracking status of a motion controller
         *
         * This function returns the current tracking status (or calibration
         * status if the controller is not calibrated yet) of a controller.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         *
         * \return One of the \ref PSMoveTracker_Status values
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveTrackerStatus psmove_tracker_get_status(IntPtr tracker, IntPtr move);

        /**
         * \brief Retrieve the next image from the camera
         *
         * This function should be called once per main loop iteration (even
         * if multiple controllers are tracked), and will grab the next camera
         * image from the camera input device.
         *
         * This function must be called before psmove_tracker_update().
         *
         * \param tracker A valid \ref PSMoveTracker handle
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_update_image(IntPtr tracker);

        /**
         * \brief Process incoming data and update tracking information
         *
         * This function tracks one or all motion controllers in the camera
         * image, and updates tracking information such as position, radius
         * and camera color.
         *
         * This function must be called after psmove_tracker_update_image().
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle (to update a single controller)
         *             or \c NULL to update all enabled controllers at once
         *
         * \return Nonzero if tracking was successful, zero otherwise
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_update(IntPtr tracker, IntPtr move);

        /**
         * \brief Draw debugging information onto the current camera image
         *
         * This function has to be called after psmove_tracker_update(), and
         * will annotate the camera image with sphere positions and other
         * information. The camera image will be modified in place, so no
         * call to psmove_tracker_update() should be carried out before the
         * next call to psmove_tracker_update_image().
         *
         * This function is used for demonstration and debugging purposes, in
         * production environments you usually do not want to use it.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         */
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_annotate(IntPtr tracker);

        /**
         * \brief Get the current camera image as backend-specific pointer
         *
         * This function returns a pointer to the backend-specific camera
         * image. Right now, the only backend supported is OpenCV, so the
         * return value will always be a pointer to an IplImage structure.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         *
         * \return A pointer to the camera image (currently always an IplImage)
         *         - the caller MUST NOT modify or free the returned object.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_tracker_get_frame(IntPtr tracker);

        /**
         * \brief Get the current camera image as 24-bit RGB data blob
         *
         * This function converts the internal camera image to a tightly-packed
         * 24-bit RGB image. The \ref PSMoveTrackerRGBImage structure is used
         * to return the image data pointer as well as the width and height of
         * the camera imaged. The size of the image data is 3 * width * height.
         *
         * The returned pixel data pointer points to tracker-internal data, and must
         * not be freed. The returned RGB data will only be valid as long as the
         * tracker exists.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         *
         * \return A \ref PSMoveTrackerRGBImage describing the RGB data and size.
         *         The RGB data is owned by the tracker, and must not be freed by
         *         the caller. The return value is valid only for the lifetime of
         *         the tracker object.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static PSMoveTrackerRGBImage psmove_tracker_get_image(IntPtr tracker);

        /**
         * \brief Get the current position and radius of a tracked controller
         *
         * This function obtains the position and radius of a controller in the
         * camera image.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param move A valid \ref PSMove handle
         * \param x A pointer to store the X part of the position, or \c NULL
         * \param y A pointer to store the Y part of the position, or \c NULL
         * \param radius A pointer to store the controller radius, or \C NULL
         *
         * \return The age of the sensor reading in milliseconds, or -1 on error
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static int psmove_tracker_get_position(IntPtr tracker, IntPtr move, ref float x, ref float y, ref float radius);

        /**
         * \brief Get the camera image size for the tracker
         *
         * This function can be used to obtain the real camera image size used
         * by the tracker. This is useful to convert the absolute position and
         * radius values to values relative to the image size if a camera is
         * used for which the size is now known. By default, the PS Eye camera
         * is used with an image size of 640x480.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param width A pointer to store the width of the camera image
         * \param height A pointer to store the height of the camera image
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_get_size(IntPtr tracker, ref int width, ref int height);

        /**
         * \brief Calculate the physical distance (in cm) of the controller
         *
         * Given the radius of the controller in the image (in pixels), this function
         * calculates the physical distance of the controller from the camera (in cm).
         *
         * By default, this function's parameters are set up for the PS Eye camera in
         * wide angle view. You can set different parameters using the function
         * psmove_tracker_set_distance_parameters().
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param radius The radius for which the distance should be calculated, the
         *               radius is returned by psmove_tracker_get_position()
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static float psmove_tracker_distance_from_radius(IntPtr tracker, float radius);

        /**
         * \brief Set the parameters for the distance mapping function
         *
         * This function sets the parameters for the Pearson VII distribution
         * function that's used to map radius values to distance values in
         * psmove_tracker_distance_from_radius(). By default, the parameters are
         * set up so that they work well for a PS Eye camera in wide angle mode.
         *
         * The function is defined as in: http://fityk.nieto.pl/model.html
         *
         * distance = height / ((1+((radius-center)/hwhm)^2 * (2^(1/shape)-1)) ^ shape)
         *
         * \param tracker A valid \ref PSMoveTracker handle
         * \param height The height parameter of the Pearson VII distribution
         * \param center The center parameter of the Pearson VII distribution
         * \param hwhm The hwhm parameter of the Pearson VII distribution
         * \param shape The shape parameter of the Pearson VII distribution
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_set_distance_parameters(IntPtr tracker, float height, float center, float hwhm, float shape);

        /**
         * \brief Destroy an existing tracker instance and free allocated resources
         *
         * This will shut down the tracker, clean up all state information for
         * tracked controller as well as close the camera device. Return values
         * for functions returning data pointing to internal tracker data structures
         * will become invalid after this function call.
         *
         * \param tracker A valid \ref PSMoveTracker handle
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_tracker_free(IntPtr tracker);

        /*-----------------------------------------------------------------------------
        //                  PSMoveFusion
        -----------------------------------------------------------------------------*/

        /**
        * \brief Create a new PS Move Fusion object
        *
        * Creates and returns a new \ref PSMoveFusion object.
        *
        * \param tracker The \ref PSMoveTracker instance from which position
        *                information should be obtained
        * \param z_near The Z coordinate of the near clipping plane
        * \param z_far The Z coordinate of the far clipping plane
        *
        * \return A new \ref PSMoveFusion handle or \c NULL on error
        **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_fusion_new(IntPtr tracker, float z_near, float z_far);

        /**
         * \brief Get a pointer to the 4x4 projection matrix
         *
         * This function returns the OpenGL projection matrix for the camera
         * used. Usually the return value can be loaded directly into the
         * GL_PROJECTION matrix of the user application using glLoadMatrix().
         *
         * \param fusion A valid \ref PSMoveFusion handle
         *
         * \return A pointer to a 16-item (4x4) float array representing
         *         the current projection matrix. The return value is only
         *         valid as long as the \ref PSMoveFusion object exists, the
         *         caller MUST NOT free() the return value.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_fusion_get_projection_matrix(IntPtr fusion);

        /**
         * \brief Get a pointer to the 4x4 model-view matrix for a controller
         *
         * This function returns the OpenGL model-view matrix for one motion
         * controller. The coordinate system origin is at the center of the
         * sphere, aligned with the controller. The returned matrix therefore
         * describes both the position and orientation of the controller in 3D
         * space. Usually the return value can be loaded directly into the
         * GL_MODELVIEW matrix of the user application using glLoadMatrix().
         *
         * \param fusion A valid \ref PSMoveFusion handle
         * \param move A valid \ref PSMove handle
         *
         * \return A pointer to a 16-item (4x4) float array representing
         *         the modelview matrix for the controller. The return value
         *         is only valid as long as the \ref PSMoveFusion object
         *         exists, the caller MUST NOT free() the return value.
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static IntPtr psmove_fusion_get_modelview_matrix(IntPtr fusion, IntPtr move);

        /**
         * \brief Get the 3D position of a controller
         *
         * This function returns the 3D position (relative to the camera)
         * of the motion controller, based on the current projection matrix.
         *
         * \param fusion A valid \ref PSMoveFusion handle
         * \param move A valid \ref PSMove handle
         * \param x A pointer to store the X part of the position vector
         * \param y A pointer to store the Y part of the position vector
         * \param z A pointer to store the Z part of the position vector
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_fusion_get_position(IntPtr fusion, IntPtr move, ref float x, ref float y, ref float z);

        /**
         * \brief Destroy an existing fusion instance and free allocated resources
         *
         * \param fusion A valid \ref PSMoveFusion handle
         **/
        [DllImport("psmoveapi_tracker.dll")]
        public extern static void psmove_fusion_free(IntPtr fusion);
    }
}
