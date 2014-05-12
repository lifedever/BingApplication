; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "每日Bing壁纸"
#define MyAppVersion "0.2.4"
#define MyAppPublisher "葛方帅"
#define MyAppURL "http://www.wincn.net/"
#define MyAppExeName "BingApplication.exe"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (生成新的GUID，点击 工具|在IDE中生成GUID。)
AppId={{12892ADF-0D7B-4117-B148-9AB8F20D9023}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\setup\License.rtf
InfoBeforeFile=F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\setup\Readme.rtf
OutputDir=F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\setup
OutputBaseFilename=setup
SetupIconFile=F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\app.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\Release\BingApplication.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\Release\app.ico"; DestDir: "{app}"; Flags: ignoreversion
; Source: "F:\Documents\Visual Studio 2013\Projects\product\BingApplication\BingApplication\bin\Release\AutoUpdate.exe"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userdesktop}\每日Bing壁纸";Filename: "{app}\BingApplication.exe"; WorkingDir: "{app}"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

