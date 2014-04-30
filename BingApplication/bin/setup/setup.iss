; 脚本用 Inno Setup 脚本向导 生成。
; 查阅文档获取创建 INNO SETUP 脚本文件详细资料！

#define MyAppName "Bing壁纸获取工具"
#define MyAppVerName "Bing壁纸获取工具 0.0.3"
#define MyAppPublisher "葛方帅"
#define MyAppURL "http://www.wincn.net"
#define MyAppExeName "BingApplication.exe"

[Setup]
; 注意: AppId 的值是唯一识别这个程序的标志。
; 不要在其他程序中使用相同的 AppId 值。
; (在编译器中点击菜单“工具 -> 产生 GUID”可以产生一个新的 GUID)
AppId={{C712ED21-2ABF-4A89-9280-DA788FC5C5E5}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\Users\gefangshuai\Desktop
OutputBaseFilename=Bing壁纸获取工具安装程序
Compression=lzma
SolidCompression=yes

[Languages]
Name: "default"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\Release\BingApplication.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\Documents\Visual Studio 2013\Projects\BingApplication\BingApplication\bin\Release\BingApplication.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享的系统文件使用 "Flags: ignoreversion" 

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
