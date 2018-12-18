using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;

public class BuildXcode
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        EditorProject(pathToBuiltProject);
        EditorPlist(pathToBuiltProject);
    }

    static void EditorProject(string _pathToBuiltProject)
    {
        // 修改工程配置
        PBXProject project = new PBXProject();
        string projPath = PBXProject.GetPBXProjectPath(_pathToBuiltProject);
        project.ReadFromString(File.ReadAllText(projPath));
        string targetGuid = project.TargetGuidByName(PBXProject.GetUnityTargetName());

        project.AddFrameworkToProject(targetGuid, "libz.tbd", true);
        project.AddFrameworkToProject(targetGuid, "libc++.tbd", true);
        project.AddFrameworkToProject(targetGuid, "libz.1.2.5.tbd", true);
        project.AddFrameworkToProject(targetGuid, "libsqlite3.0.tbd", true);

        project.AddFrameworkToProject(targetGuid, "iAd.framework", true);
        project.AddFrameworkToProject(targetGuid, "UIKit.framework", true);
        project.AddFrameworkToProject(targetGuid, "OpenAL.framework", true);
        project.AddFrameworkToProject(targetGuid, "Security.framework", true);
        project.AddFrameworkToProject(targetGuid, "OpenGLES.framework", true);
        project.AddFrameworkToProject(targetGuid, "StoreKit.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreText.framework", true);
        project.AddFrameworkToProject(targetGuid, "CFNetwork.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreMedia.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreVideo.framework", true);
        project.AddFrameworkToProject(targetGuid, "AdSupport.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreMotion.framework", true);
        project.AddFrameworkToProject(targetGuid, "Foundation.framework", true);
        project.AddFrameworkToProject(targetGuid, "QuartzCore.framework", true);
        project.AddFrameworkToProject(targetGuid, "MediaPlayer.framework", true);
        project.AddFrameworkToProject(targetGuid, "AudioToolbox.framework", true);
        project.AddFrameworkToProject(targetGuid, "AVFoundation.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", true);
        project.AddFrameworkToProject(targetGuid, "CoreFoundation.framework", true);
        project.AddFrameworkToProject(targetGuid, "GameController.framework", true);
        project.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", true);

        project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(targetGuid, "OTHER_LDFLAGS", GetOtherLinkerFlags());
        project.SetBuildProperty(targetGuid, "LIBRARY_SEARCH_PATHS", GetLibrarySearchPaths());
        project.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", GetFrameworksSearchPaths());

        // 保存工程修改
        File.WriteAllText(projPath, project.WriteToString());
    }

    static void EditorPlist(string _pathToBuiltProject)
    {
        // 修改 plist
        string plistPath = Path.Combine(_pathToBuiltProject, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict rootDict = plist.root;

        // 一些权限声明
        rootDict.SetString("NSCameraUsageDescription", "我们需要使用摄像头权限");

        PlistElementDict tempFtxUrl = rootDict.CreateDict("FTXPluginsParams");
        tempFtxUrl.SetString("appId", "328");
        tempFtxUrl.SetString("channelId", "10066");
        tempFtxUrl.SetBoolean("debug", true);

        File.WriteAllText(plistPath, plist.WriteToString());
    }

    static string GetOtherLinkerFlags()
    {
        string tempBase = "-weak_framework CoreMotion -weak-lSystem";
        string tempAppStore = " -force_load " + '"' + "$(SRCROOT)/Libraries/Plugins/iOS/sdkLib/libFTXplugin_AppStore.a" + '"';
        string tempSdkIOS = " -force_load " + '"' + "$(SRCROOT)/Frameworks/Plugins/iOS/sdkLib/FTXSdkIOS.framework/FTXSdkIOS" + '"';
        return tempBase + tempSdkIOS + tempAppStore;
    }

    static string GetLibrarySearchPaths()
    {
        string tempIOS = " $(SRCROOT)/Libraries/Plugins/iOS";
        string tempLibrary = " $(inherited) $(SRCROOT) $(SRCROOT)/Libraries";
        string tempSdkLib = " $(PROJECT_DIR)/Libraries/Plugins/iOS/sdkLib " + '"' + "$(SRCROOT)/Libraries" + '"';
        return tempLibrary + tempIOS + tempSdkLib;
    }

    static string GetFrameworksSearchPaths()
    {
        string tempSdkLib = "$(inherited) $(PROJECT_DIR)/Frameworks/Plugins/iOS/sdkLib";
        string tempFramework = '"' + "$(SRCROOT)/Frameworks/Plugins/iOS/sdkLib/FTXSdkIOS.framework" + '"';
        return tempSdkLib + tempFramework;
    }
}