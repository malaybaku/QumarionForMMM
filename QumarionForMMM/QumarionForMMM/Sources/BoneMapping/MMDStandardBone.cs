using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QumarionForMMM
{
    public static class MMDStandardBones
    {
        public static Bone GetBone(Model model, MMDStandardBone bone)
            => model.Bones.FirstOrDefault(b => b.Name == _mmdBoneNames[bone]);

        private static readonly Dictionary<MMDStandardBone, string> _mmdBoneNames = new Dictionary<MMDStandardBone, string>()
        {
            { MMDStandardBone.Center, "センター" },
            //TODO: これ適切かなあ？
            { MMDStandardBone.Hip, "全ての親" },

            { MMDStandardBone.Spine, "上半身" },
            { MMDStandardBone.Spine2, "上半身2" },
            { MMDStandardBone.Neck, "首" },
            { MMDStandardBone.Head, "頭" },

            { MMDStandardBone.LeftShoulder, "左肩" },
            { MMDStandardBone.LeftUpperArm, "左腕" },
            { MMDStandardBone.LeftArmRoll, "左腕捩" },
            { MMDStandardBone.LeftElbow, "左ひじ" },
            { MMDStandardBone.LeftHandRoll, "左手捩" },
            { MMDStandardBone.LeftHand, "左手首" },

            { MMDStandardBone.RightShoulder, "右肩" },
            { MMDStandardBone.RightUpperArm, "右腕" },
            { MMDStandardBone.RightArmRoll, "右腕捩" },
            { MMDStandardBone.RightElbow, "右ひじ" },
            { MMDStandardBone.RightHandRoll, "右手捩" },
            { MMDStandardBone.RightHand, "右手首" },

            { MMDStandardBone.LeftLeg, "左足" },
            { MMDStandardBone.LeftKnee, "左ひざ" },
            { MMDStandardBone.LeftFoot, "左足首" },
            { MMDStandardBone.LeftToe, "左つま先" },

            { MMDStandardBone.RightLeg, "右足" },
            { MMDStandardBone.RightKnee, "右ひざ" },
            { MMDStandardBone.RightFoot, "右足首" },
            { MMDStandardBone.RightToe, "右つま先" },

            //NOTE: "IK"が全角なのは書き損じではない。            
            { MMDStandardBone.LeftFootIK, "左足ＩＫ" },
            { MMDStandardBone.RightFootIK, "右足ＩＫ" }
        };

    }

    public enum MMDStandardBone
    {
        Center,

        Hip,
        Spine,
        Spine2,
        Neck,
        Head,

        LeftShoulder,
        LeftUpperArm,
        LeftArmRoll,
        LeftElbow,
        LeftHandRoll,
        LeftHand,

        RightShoulder,
        RightUpperArm,
        RightArmRoll,
        RightElbow,
        RightHandRoll,
        RightHand,

        //TODO: IKとFKの使い分けがあまりキレイに整備されていない
        LeftLeg,
        LeftKnee,
        LeftFoot,
        LeftFootIK,
        LeftToe,

        RightLeg,
        RightKnee,
        RightFoot,
        RightFootIK,
        RightToe
    }
}
