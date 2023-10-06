# PSMoveManager

## 概要

- PS Move API の .NET 環境用実装。
- PS Move のセンサー情報の取得や、LED、振動などの操作が可能。
(USB 接続の場合は、LED と振動操作のみ)

## 動作環境

- Windows 10 (Windows 11 は未確認)
- .NET Standard 2.0

## 前提条件

- PS Move を Bluetooth 接続しておく。(USB 接続の場合は、一部の機能のみ使用できる)
- PlayStation Eye を USB 接続しておく。(接続していない場合は、トラッキングは利用不可)

## 準備するもの

- PC (Windows 10)
- PS Move
- PlayStation Eye (PS Move のトラッキングに使用)
- PS Move API (PS Move API 4.0.12 Windows ver)

## PS Move API

PS Move を Windows 上で扱うための 非公式 API。

- ドキュメント
https://psmoveapi.readthedocs.io/en/latest/camera.html
- GitHub
https://github.com/thp/psmoveapi/tree/master

## PS Move を PC と Bluetooth 接続する

### 簡易

1. PC と PS Move を一旦、有線接続。
2. PC と PS Move を無線接続。
3. PS Move をキャリブレーション。

### 詳細

1. PS Move API の[バイナリ](https://github.com/thp/psmoveapi/releases) (psmoveapi-4.0.12-windows-msvc2017-xxx.zip) をダウンロード。
付属の psmove.exe は CLI ツール。ターミナルを管理者権限で開いて利用。
2. PS Move と PC を USB ケーブルで有線接続する。
また、PC に Bluetooth が付いていない場合は、Bluetooth アダプターを接続しておく。
3. `psmove.exe pair` コマンドを実行する。PC に Bluetooth 機能がないとエラーが出る。
    1. **Unplug the controller** と表示されたら、PS Move から USB ケーブルを外し、PS ボタン (PSマークが描かれたボタン) を押す。
    2. PS Move 下部の赤色 LED が点滅するので、消灯したら再度 PS ボタンを押す。
    3. **Pairing of #X succeeded!** というメッセージが出て赤色 LED が点灯し続けるまで、b の操作を繰り返す。(PS ボタンを10秒間押し続けると、電源ごと消えるので注意)
4. `psmove.exe calibrate` コマンドを実行する。
    1. PS Move の先端部が光って **Calibrating PS Move #X** と表示されたら、PS Move を様々な方向に回転させる。
    2. a の操作を表示されている数値が止まるまで続けた後、Move ボタン (中央にある一番大きいボタン) を押す。
    3. **Stand the controller** と表示されたら、PS Move を天井に向け Move ボタンを自分自身に正対させた状態で押す。**Finished PS Move #X** と表示されれば無事に完了。

## **PlayStation Eye** を PC と USB 接続する

1. PlayStation Eye の[ドライバー](https://archive.org/download/CLEyeDriver5.3.0.0341Emuline/) (CL-Eye-Driver-5.3.0.0341-Emuline.exe) をインストールする。
**ドライバーをインストールする前に PlayStation Eye と PC を USB接続すると、うまく動作しないという報告あり。**
2. PlayStation Eye のケーブルを PC の USB ポートに接続する。
3. ドライバーと一緒にインストールされた CL-eye Test を実行。無事に PlayStation Eye の画面が表示されれば接続に成功。(別のデバイスの画面が表示される場合は、メニューの Devices から PlayStation Eye を選択する)

## 注意点

- 室内が暗すぎると、PS Move を PlayStation Eye が認識しない。ある程度、部屋を明るくすること。
- PlayStation Eye (USB2.0) を USB3.0 ポートに接続すると認識しないという報告あり。
- PS Move は型番によっては、Windows 上で地磁気センサーが取得できない場合がある。(PS3 発売当時の PS Move だと比較的問題ない模様。要確認)