using Newtonsoft.Json.Linq;
using StereoKit;

namespace Nazar.Extension.AIWorldGenerator;

internal class JSONConverter
{
    //Vec3
    public static Vec3 FromJSONVec3(JObject someData)
    {
        Vec3 result = new((float) someData.GetValue("x"), (float) someData.GetValue("y"),
            (float) someData.GetValue("z"));
        return result;
    }

    public static JObject ToJSON(Vec3 someData)
    {
        JObject result = new();
        result.Add("x", someData.x);
        result.Add("y", someData.y);
        result.Add("z", someData.z);
        return result;
    }

    //Color
    public static Color FromJSONColor(JObject someData)
    {
        Color result = new((float) someData.GetValue("r"), (float) someData.GetValue("g"),
            (float) someData.GetValue("b"));
        return result;
    }

    public static JObject ToJSON(Color someData)
    {
        JObject result = new();
        result.Add("r", someData.r);
        result.Add("g", someData.g);
        result.Add("b", someData.b);
        return result;
    }

    //Quaternion
    public static Quat FromJSONQuat(JObject someData)
    {
        Quat result = new((float) someData.GetValue("x"), (float) someData.GetValue("y"),
            (float) someData.GetValue("z"), (float) someData.GetValue("w"));
        return result;
    }

    public static JObject ToJSON(Quat someData)
    {
        JObject result = new();
        result.Add("x", someData.x);
        result.Add("y", someData.y);
        result.Add("z", someData.z);
        result.Add("w", someData.w);
        return result;
    }
}