# PSMoveManager

## �T�v

- PS Move API �� .NET ���p�����B
- PS Move �̃Z���T�[���̎擾��ALED�A�U���Ȃǂ̑��삪�\�B
(USB �ڑ��̏ꍇ�́ALED �ƐU������̂�)

## �����

- Windows 10 (Windows 11 �͖��m�F)
- .NET Standard 2.0

## �O�����

- PS Move �� Bluetooth �ڑ����Ă����B(USB �ڑ��̏ꍇ�́A�ꕔ�̋@�\�̂ݎg�p�ł���)
- PlayStation Eye �� USB �ڑ����Ă����B(�ڑ����Ă��Ȃ��ꍇ�́A�g���b�L���O�͗��p�s��)

## �����������

- PC (Windows 10)
- PS Move
- PlayStation Eye (PS Move �̃g���b�L���O�Ɏg�p)
- PS Move API (PS Move API 4.0.12 Windows ver)

## PS Move API

PS Move �� Windows ��ň������߂� ����� API�B

- �h�L�������g
https://psmoveapi.readthedocs.io/en/latest/camera.html
- GitHub
https://github.com/thp/psmoveapi/tree/master

## PS Move �� PC �� Bluetooth �ڑ�����

### �Ȉ�

1. PC �� PS Move ����U�A�L���ڑ��B
2. PC �� PS Move �𖳐��ڑ��B
3. PS Move ���L�����u���[�V�����B

### �ڍ�

1. PS Move API ��[�o�C�i��](https://github.com/thp/psmoveapi/releases) (psmoveapi-4.0.12-windows-msvc2017-xxx.zip) ���_�E�����[�h�B
�t���� psmove.exe �� CLI �c�[���B�^�[�~�i�����Ǘ��Ҍ����ŊJ���ė��p�B
2. PS Move �� PC �� USB �P�[�u���ŗL���ڑ�����B
�܂��APC �� Bluetooth ���t���Ă��Ȃ��ꍇ�́ABluetooth �A�_�v�^�[��ڑ����Ă����B
3. `psmove.exe pair` �R�}���h�����s����BPC �� Bluetooth �@�\���Ȃ��ƃG���[���o��B
    1. **Unplug the controller** �ƕ\�����ꂽ��APS Move ���� USB �P�[�u�����O���APS �{�^�� (PS�}�[�N���`���ꂽ�{�^��) �������B
    2. PS Move �����̐ԐF LED ���_�ł���̂ŁA����������ēx PS �{�^���������B
    3. **Pairing of #X succeeded!** �Ƃ������b�Z�[�W���o�ĐԐF LED ���_����������܂ŁAb �̑�����J��Ԃ��B(PS �{�^����10�b�ԉ���������ƁA�d�����Ə�����̂Œ���)
4. `psmove.exe calibrate` �R�}���h�����s����B
    1. PS Move �̐�[���������� **Calibrating PS Move #X** �ƕ\�����ꂽ��APS Move ��l�X�ȕ����ɉ�]������B
    2. a �̑����\������Ă��鐔�l���~�܂�܂ő�������AMove �{�^�� (�����ɂ����ԑ傫���{�^��) �������B
    3. **Stand the controller** �ƕ\�����ꂽ��APS Move ��V��Ɍ��� Move �{�^�����������g�ɐ��΂�������Ԃŉ����B**Finished PS Move #X** �ƕ\�������Ζ����Ɋ����B

## **PlayStation Eye** �� PC �� USB �ڑ�����

1. PlayStation Eye ��[�h���C�o�[](https://archive.org/download/CLEyeDriver5.3.0.0341Emuline/) (CL-Eye-Driver-5.3.0.0341-Emuline.exe) ���C���X�g�[������B
**�h���C�o�[���C���X�g�[������O�� PlayStation Eye �� PC �� USB�ڑ�����ƁA���܂����삵�Ȃ��Ƃ����񍐂���B**
2. PlayStation Eye �̃P�[�u���� PC �� USB �|�[�g�ɐڑ�����B
3. �h���C�o�[�ƈꏏ�ɃC���X�g�[�����ꂽ CL-eye Test �����s�B������ PlayStation Eye �̉�ʂ��\�������ΐڑ��ɐ����B(�ʂ̃f�o�C�X�̉�ʂ��\�������ꍇ�́A���j���[�� Devices ���� PlayStation Eye ��I������)

## ���ӓ_

- �������Â�����ƁAPS Move �� PlayStation Eye ���F�����Ȃ��B������x�A�����𖾂邭���邱�ƁB
- PlayStation Eye (USB2.0) �� USB3.0 �|�[�g�ɐڑ�����ƔF�����Ȃ��Ƃ����񍐂���B
- PS Move �͌^�Ԃɂ���ẮAWindows ��Œn���C�Z���T�[���擾�ł��Ȃ��ꍇ������B(PS3 ���������� PS Move ���Ɣ�r�I���Ȃ��͗l�B�v�m�F)