using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Karambolo.PO;

namespace Ohayo
{
    public class SourceEntry
    {
        public SourceEntry(POSourceReference sourceReference, DirectoryInfo basePath)
        {
            SourceReference = sourceReference;
            BasePath = basePath;
        }
        public POSourceReference SourceReference { get; }
        public DirectoryInfo BasePath { get; }
        public override string ToString() => SourceReference.ToString();
    }
}