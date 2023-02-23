using StereoKit.Framework;

namespace Nazar.SKit.Framework;

public interface IConfigurableStepper : IStepper
{
    Type GetConfigUi();

}