using System;
using StereoKit.Framework;

namespace NazAR.Common;

public interface IUiStepper : IStepper
{
    Type GetUserInterface();

}