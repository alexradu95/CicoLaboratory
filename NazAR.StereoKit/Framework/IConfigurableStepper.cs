using StereoKit.Framework;

namespace NazAR.StereoKit.Framework;

public interface IConfigurableStepper : IStepper
{
    Type GetConfigUi();

}