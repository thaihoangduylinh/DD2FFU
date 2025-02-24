﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.Imaging.LocateIdentifier
// Assembly: ImageStorageServiceManaged, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: BF244519-1EED-4829-8682-56E05E4ACE17
// Assembly location: C:\Users\gus33000\source\repos\DD2FFU\DD2FFU\libraries\imagestorageservicemanaged.dll

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Decomp.Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Decomp.Microsoft.WindowsPhone.Imaging
{
    public class LocateIdentifier : BaseIdentifier, IDeviceIdentifier
    {
        
        public enum LocateType : uint
        {
            BootElement,
            String
        }

         public LocateType Type { get; set; }

         public uint ElementType { get; set; }

         public uint ParentOffset { get; set; }

        public string Path { get; set; }

        public void ReadFromStream(BinaryReader reader)
        {
            Type = (LocateType) reader.ReadUInt32();
            ElementType = reader.ReadUInt32();
            ParentOffset = reader.ReadUInt32();
            Path = reader.ReadString();
            if (Type == LocateType.BootElement)
                throw new ImageStorageException("Not supported.");
        }

        public void WriteToStream(BinaryWriter writer)
        {
            throw new ImageStorageException(string.Format("{0}: This function isn't implemented.",
                MethodBase.GetCurrentMethod().Name));
        }

        
        public void LogInfo(IULogger logger, int indentLevel)
        {
            var str = new StringBuilder().Append(' ', indentLevel).ToString();
            logger.LogInfo(str + "Identifier: Locate");
        }

         public uint Size => 0;
    }
}