using Newtonsoft.Json.Linq;
using StereoKit;

namespace Nazar.Extensions.AIWorldGenerator;

public class WorldObject
{
    private Color myColor;
    private Model myModel;
    private Pose myPose = Pose.Identity;
    private Vec3 myScale = Vec3.One;

    private string
        myShape; //Can be "cube", "cylinder", "plane", "rounded cube". See generate functions at https://stereokit.net/Pages/StereoKit/Mesh.html

    public WorldObject(int anId, JObject someData) //JObject is a JSON object
    {
        myId = anId;

        UpdateFromJSON(someData);
    }

    public int myId { get; }

    public void UpdateFromJSON(JObject someData)
    {
        someData.TryGetValue("position", out JToken JPos);
        someData.TryGetValue("scale", out JToken JScale);
        someData.TryGetValue("shape", out JToken JShape);
        someData.TryGetValue("color", out JToken JColor);

        //Position
        if (JPos != null) myPose.position = JSONConverter.FromJSONVec3((JObject) JPos);
        //Scale
        if (JScale != null) myScale = JSONConverter.FromJSONVec3((JObject) JScale);
        //Mesh
        if (JShape != null)
        {
            string str = JShape.ToString();
            myShape = str;

            switch (str)
            {
                case "cube":
                    myModel = Model.FromMesh(Mesh.Cube, Material.UI);
                    break;
                case "sphere":
                    myModel = Model.FromMesh(Mesh.Sphere, Material.UI);
                    break;
                case "cylinder":
                {
                    Mesh cylinder = Mesh.GenerateCylinder(1.0f, 1.0f, Vec3.Up);
                    myModel = Model.FromMesh(cylinder, Material.UI);
                    break;
                }
                //continue with more meshes
                //default cube
                default:
                    myModel = Model.FromMesh(Mesh.Cube, Material.UI);
                    break;
            }
        }

        //Color
        if (JColor != null)
        {
            Color color = JSONConverter.FromJSONColor((JObject) JColor);
            myColor = color;
        }
    }

    private JObject ToJson()
    {
        JObject result = new()
        {
            {"id", myId},
            {"position", JSONConverter.ToJSON(myPose.position)},
            {"scale", JSONConverter.ToJSON(myScale)},
            {"shape", myShape},
            {"color", JSONConverter.ToJSON(myColor)}
        };

        return result;
    }

    public void Draw()
    {
        Vec3 worldPositionOffset = new(0, -0.5f, -1);
        Vec3 worldScaleOffset = new(0.5f, 0.5f, 0.5f);
        Matrix worldOffset = Matrix.TS(worldPositionOffset, worldScaleOffset);

        myModel.Draw(myPose.ToMatrix(myScale) * worldOffset, myColor);
    }
}