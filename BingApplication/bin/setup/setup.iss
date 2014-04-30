; 脚本用 Inno Setup 脚本向导 生成。
; 查阅文档获取创建 INNO SETUP 脚本文件详细资料！

#define MyAppName "Bing壁纸获取程序"
#define MyAppVerName "Bing壁纸获取程序 0.0.3"
#define MyAppPublisher "葛方帅"
#define MyAppURL "http://www.wincn.net/"
#define MyAppExeName "BingApplication.exe"

[Setup]
; 注意: AppId 的值是唯一识别这个程序的标志。
; 不要在其他程序中使用相同的 AppId 值。
; (在编译器中点击菜单“工具 -> 产生 GUID”可以产生一个新的 GUID)
AppId={{02FCCCDA-83E6-4152-AA38-C82E6B25DE52}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
InfoBeforeFile=F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\setup\before.txt
OutputDir=F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\setup
OutputBaseFilename=setup
SetupIconFile=F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\1140395.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "default"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\Release\BingApplication.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\Release\BingApplication.exe.Config"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享的系统文件使用 "Flags: ignoreversion" 

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
