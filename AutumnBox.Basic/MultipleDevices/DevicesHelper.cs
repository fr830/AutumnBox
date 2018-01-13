/* =============================================================================*\
*
* Filename: DevicesHelper.cs
* Description:  Static Functions About Device(s)
*
* Version: 1.0
* Created: 8/19/2017 03:42:33(UTC+8:00)
* Compiler: Visual Studio 2017
* 
* Author: zsh2401
* Company: I am free man
*
\* =============================================================================*/
using AutumnBox.Support.CstmDebug;

namespace AutumnBox.Basic.MultipleDevices
{
    [LogProperty(TAG = "DevicesHelper")]
    /// <summary>
    /// 关于设备的一些静态函数
    /// </summary>
    public static class DevicesHelper
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns>设备列表</returns>
        public static DevicesList GetDevices()
        {
            return new DevicesGetter().GetDevices();
        }
    }
}