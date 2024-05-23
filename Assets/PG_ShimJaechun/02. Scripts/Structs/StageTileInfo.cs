using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{ 
    [Serializable]
    public struct TileDepthInfo
    {
        public List<ChangeableTile> tileList;

        public TileDepthInfo(List<ChangeableTile> tileList)
        {
            this.tileList = tileList;
        }
    }

    [Serializable]
    public struct StageTileInfo
    {
        public List<TileDepthInfo> depthList;

        public StageTileInfo(List<TileDepthInfo> depthList)
        {
            this.depthList = depthList;
        }
    }
}
