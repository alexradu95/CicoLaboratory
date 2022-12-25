using Android.App;
using Android.Runtime;
using System;

namespace CicoLaboratory;

[Application]
public class MainApplication : Application
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}
}
