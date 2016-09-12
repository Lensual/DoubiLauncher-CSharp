using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubiLauncher_CSharp
{
    /// <summary>
    ///  MCJS数据结构类
    /// </summary>
    public class MCJsonStruct
    {
        #region "启动参数类结构声明"
        /// <summary>
        /// 启动参数
        /// </summary>
        public class LaunchArguments
        {
            public string id;
            public string time;
            public string releaseTime;
            public string type;
            /// <summary>
            /// 给主类传递的参数
            /// </summary>
            public string minecraftArguments;
            /// <summary>
            /// 主类
            /// </summary>
            public string mainClass;
            public string minimumLauncherVersion;
            public string inheritsFrom;
            public string assets;
            public string jar;
            /// <summary>
            /// libraries数组
            /// </summary>
            public libraries[] libraries;
        }
        #endregion
        #region libraries类结构声明
        /// <summary>
        /// 启动所依赖的库文件
        /// </summary>
        public class libraries
        {
            /// <summary>
            /// jar包的名字和路径
            /// </summary>
            public string name;
            public string url;
            /// <summary>
            /// 服务端需要
            /// </summary>
            public string serverreq;
            /// <summary>
            /// 客户端需要
            /// </summary>
            public string clientreq;
            /// <summary>
            /// 校验和
            /// </summary>
            public string[] checksums;
            /// <summary>
            /// 说明
            /// </summary>
            public string comment;
            /// <summary>
            /// 规则
            /// </summary>
            public rules[] rules;
            public natives natives;
        }
        #region rules类结构声明
        /// <summary>
        /// 库文件依赖规则
        /// </summary>
        public class rules
        {
            /// <summary>
            /// 该规则成立应做的动作
            /// </summary>
            public string action;
            /// <summary>
            /// 条件 系统类型
            /// </summary>
            public os os;
        }
        #region os类结构声明
        /// <summary>
        /// 系统类型
        /// </summary>
        public class os
        {
            /// <summary>
            /// 系统名字
            /// </summary>
            public string name;
        }
        #endregion

        #endregion

        #region natives类结构声明
        public class natives
        {
            public string linux;
            public string windows;
            public string osx;
        }
        #endregion

        #endregion

    }
}
