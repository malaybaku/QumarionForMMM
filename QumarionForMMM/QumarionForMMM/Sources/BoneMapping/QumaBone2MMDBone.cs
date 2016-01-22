using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    public static class QumaBone2MMDBone
    {
        //public static string GetMMDBoneName(StandardPSBones qumaBone) => _quma2mmd[qumaBone];
        public static MMDStandardBone GetMMDBone(StandardPSBones qumaBone) => _quma2mmd[qumaBone];
        public static StandardPSBones GetQumaBone(MMDStandardBone mmdBone) => _mmd2quma[mmdBone];


        static QumaBone2MMDBone()
        {
            _mmd2quma = new Dictionary<MMDStandardBone, StandardPSBones>()
            {
                { MMDStandardBone.Hip, StandardPSBones.Hips },
                { MMDStandardBone.Spine, StandardPSBones.Spine },
                { MMDStandardBone.Spine2, StandardPSBones.Spine2 },
                { MMDStandardBone.Neck, StandardPSBones.Neck },
                { MMDStandardBone.Head, StandardPSBones.Head },

                //TODO: 「捩」系のボーンどうしましょうねえ。
                { MMDStandardBone.LeftShoulder, StandardPSBones.LeftShoulder },
                { MMDStandardBone.LeftUpperArm, StandardPSBones.LeftArm },
                { MMDStandardBone.LeftElbow, StandardPSBones.LeftForeArm },
                { MMDStandardBone.LeftHand, StandardPSBones.LeftHand },

                { MMDStandardBone.RightShoulder, StandardPSBones.RightShoulder },
                { MMDStandardBone.RightUpperArm, StandardPSBones.RightArm },
                { MMDStandardBone.RightElbow, StandardPSBones.RightForeArm },
                { MMDStandardBone.RightHand, StandardPSBones.RightHand },


                { MMDStandardBone.LeftLeg, StandardPSBones.LeftUpLeg },
                { MMDStandardBone.LeftKnee, StandardPSBones.LeftLeg },
                { MMDStandardBone.LeftToe, StandardPSBones.LeftToeBase },

                { MMDStandardBone.RightLeg, StandardPSBones.RightUpLeg },
                { MMDStandardBone.RightKnee, StandardPSBones.RightLeg },
                { MMDStandardBone.RightToe, StandardPSBones.RightToeBase },

                //NOTE: これだとIKじゃない方のLeftFoot/RightFootどうするか微妙に困るんだけど
                //どうしようねえ。まあIK任せで放置してもいいが。
                { MMDStandardBone.LeftFootIK, StandardPSBones.LeftFoot },
                { MMDStandardBone.RightFootIK, StandardPSBones.RightFoot },

            };


            _quma2mmd = _mmd2quma.ToDictionary(pair => pair.Value, pair => pair.Key);

        }

        //NOTE: MMDには「左腕捩」「右腕捩」「左手捩」「右手捩」とか足のIKがあるので
        //単純にForward Kinematicのマッピングしても不十分なことに注意せよ。
        private static readonly Dictionary<MMDStandardBone, StandardPSBones> _mmd2quma;

        private static readonly Dictionary<StandardPSBones, MMDStandardBone> _quma2mmd;

    }
}
