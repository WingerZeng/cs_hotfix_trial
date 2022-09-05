﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.Audio;

namespace HybridCLR.Editor
{
    public static partial class BuildConfig
    {

        /// <summary>
        /// 所有热更新dll列表。放到此列表中的dll在打包时OnFilterAssemblies回调中被过滤。
        /// </summary>
        public static List<string> HotUpdateAssemblies { get; } = new List<string>
        {
            "HotFix.dll",
            "Mod.dll"
        };

        public static List<string> AOTMetaAssemblies { get; } = new List<string>()
        {
        };

        public static List<string> AssetBundleFiles { get; } = new List<string>
        {
            "common",
        };
    }
}
