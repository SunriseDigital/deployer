using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace deployer.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      Process proc = new System.Diagnostics.Process();
      SecureString ssPwd = new System.Security.SecureString();
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.FileName = "cmd.exe";
      //proc.StartInfo.Arguments = "/user:Administrator";
      proc.StartInfo.RedirectStandardInput = true;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      //proc.StartInfo.CreateNoWindow = true;
      //proc.StartInfo.Verb = "runas";
      //proc.StartInfo.Arguments = "/env /user:Administrator cmd /K whoami";
      //proc.StartInfo.Domain = "miyatawindows10";
      //proc.StartInfo.UserName = "Administrator";
      //var password = "password";
      //for (int x = 0; x < password.Length; x++)
      //{
      //  ssPwd.AppendChar(password[x]);
      //}
      //proc.StartInfo.Password = ssPwd;
      
      proc.Start();

      //proc.StandardInput.WriteLine("whoami");
      proc.StandardInput.WriteLine(string.Format(@"powershell -Command ""{0}""", "New-EventLog -LogName Test -Source Foo"));
      proc.StandardInput.Flush();
      proc.StandardInput.Close();

      Sdx.Context.Current.Debug.Log(proc.StandardOutput.ReadToEnd());
      Sdx.Context.Current.Debug.Log(proc.StandardError.ReadToEnd());

      proc.WaitForExit();
      
      return View();
    }

    public ActionResult Test()
    {
      Process proc = new System.Diagnostics.Process();
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.FileName = "git.exe";
      proc.StartInfo.Arguments = "clone git@github.com:gomo/gcommand.git";
      proc.StartInfo.RedirectStandardInput = true;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      proc.StartInfo.StandardOutputEncoding = Encoding.UTF8;
      proc.StartInfo.WorkingDirectory = @"C:\Projects";
      proc.Start();

      //proc.StandardInput.WriteLine("whoami");
      //proc.StandardInput.WriteLine(string.Format(@"powershell -Command ""{0}""", "New-EventLog -LogName Test -Source Moo"));
      //proc.StandardInput.WriteLine(string.Format(@"""%ProgramFiles%\Git\bin\bash.exe"" --login -c ""{0}""", "git clone git@github.com:gomo/gcommand.git"));
      //proc.StandardInput.Flush();
      //proc.StandardInput.Close();

      

      Sdx.Context.Current.Debug.Log(proc.StandardOutput.ReadToEnd());
      Sdx.Context.Current.Debug.Log(proc.StandardError.ReadToEnd());

      proc.WaitForExit();

      return View();
    }
  }
}