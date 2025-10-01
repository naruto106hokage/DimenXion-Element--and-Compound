using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dpvr;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class AppManager
        {
            public static int succeeded = 1;
            public static int installFailedAlreadyExists = -1;
            public static int installFailedInvalidApk = -2;
            public static int installFailedOlderSdk = -12;
            public static int installFailedCpuAbiIncompatible = -16;
            public static int installParseFailedInconsistentCertificates = -104;
            public static int uninstallFailedInternalError = -1;
            public static int uninstallFailedDevicePolicyManager = -2;
            public static int uninstallFailedUnknownPkg = -10;

            private static InstallApkCallback installApkCallback;
            private static UninstallApkCallback uninstallApkCallback;

            public static void GoToApp(string packageName)
            {
                bool success = DpvrDeviceManager.GetDeviceManager().Call<bool>("goToApp", packageName);
                Debug.Log("DEBUG goto app : " + success);
            }

            public static void CloseApp(string packageName)
            {
                GetDeviceManager().Call("closeApp", packageName);
            }

            public static void InstallApk(string apkPath,
                                      OnPackageInstallSuccessHandler successCallback = null,
                                      OnPackageInstallErrorHandler errorCallback = null)
            {
                installApkCallback = new InstallApkCallback(successCallback, errorCallback);
                GetDeviceManager().Call("installApk", apkPath, installApkCallback);
            }

            public delegate void OnPackageInstallSuccessHandler(string apkPath, string packageName);
            public delegate void OnPackageInstallErrorHandler(string apkPath, int errorCode);
            class InstallApkCallback : AndroidJavaProxy
            {
                OnPackageInstallSuccessHandler successHandler;
                OnPackageInstallErrorHandler errorHandler;
                public InstallApkCallback(OnPackageInstallSuccessHandler successCallback,
                                          OnPackageInstallErrorHandler errorCallback)
                                        : base("com.dpvr.DeviceManager.PluginCallback$PackageInstallCallback")
                {
                    successHandler = successCallback;
                    errorHandler = errorCallback;
                }

                public void OnSuccess(string apkPath, string packageName)
                {
                    if (successHandler != null)
                        successHandler(apkPath, packageName);
                }

                public void OnError(string apkPath, int errorCode)
                {
                    if (errorHandler != null)
                        errorHandler(apkPath, errorCode);
                }
            }

            public static void UninstallApk(string packageName,
                                        OnPackageUninstallSuccessHandler successCallback = null,
                                        OnPackageUninstallErrorHandler errorCallback = null)
            {
                uninstallApkCallback = new UninstallApkCallback(successCallback, errorCallback);
                GetDeviceManager().Call("uninstallApk", packageName, uninstallApkCallback);
            }

            public delegate void OnPackageUninstallSuccessHandler(string packageName);
            public delegate void OnPackageUninstallErrorHandler(string packageName, int errorCode);
            class UninstallApkCallback : AndroidJavaProxy
            {
                OnPackageUninstallSuccessHandler successHandler;
                OnPackageUninstallErrorHandler errorHandler;
                public UninstallApkCallback(OnPackageUninstallSuccessHandler successCallback,
                                            OnPackageUninstallErrorHandler errorCallback)
                                            : base("com.dpvr.DeviceManager.PluginCallback$PackageUninstallCallback")
                {
                    successHandler = successCallback;
                    errorHandler = errorCallback;
                }

                public void OnSuccess(string packageName)
                {
                    Debug.Log(packageName + " packageUninstall!");
                    if (successHandler != null)
                        successHandler(packageName);
                }

                public void OnError(string packageName, int errorCode)
                {
                    if (errorHandler != null)
                        errorHandler(packageName, errorCode);
                }
            }

            public static string[] GetInstalledAppList(bool onlyLauncher = true)
            {
                return GetDeviceManager().Call<string[]>("getInstalledPkgNameList", onlyLauncher);
            }

            public static string GetAppNameByPkgname(string pkgname)
            {
                return GetDeviceManager().Call<string>("getAppNameByPkgname", pkgname);
            }

            public static byte[] GetAppIconByPkgname(string pkgname)
            {
                return GetDeviceManager().Call<byte[]>("getApkIconByPkgname", pkgname);
            }
        }
    }
}