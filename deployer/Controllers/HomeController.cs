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

    public ActionResult GitLib()
    {
      var userName = "***";
      var password = "***";
      using (var repo = new LibGit2Sharp.Repository(@"C:\Projects\test-manage-on-server"))
      {
        //現在の最新のコミットを取得しておく
        var currentCommit = (LibGit2Sharp.Commit)repo.Commits.Take(1).First();

        //PULLの準備
        LibGit2Sharp.PullOptions options = new LibGit2Sharp.PullOptions();
        options.FetchOptions = new LibGit2Sharp.FetchOptions();
        options.FetchOptions.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler((url, usernameFromUrl, types) =>
          new LibGit2Sharp.UsernamePasswordCredentials{Username = userName, Password = password}
        );
        var result = repo.Network.Pull(new LibGit2Sharp.Signature(userName, "miyata@sincere-co.com", new DateTimeOffset(DateTime.Now)), options);

        var display = new Dictionary<string, object>();
        display["Updating"] = currentCommit.Id + "..." + result.Commit.Id;
        display["Status"] = result.Status;

        var patch = repo.Diff.Compare<LibGit2Sharp.Patch>(currentCommit.Tree, result.Commit.Tree); // Difference
        display["Files"]  = patch.Select(entry =>
        {
          return new Dictionary<string, object> { 
            {"path", entry.Path},
            {"status", entry.Status},
            {"mode", entry.Mode},
            {"+", entry.LinesAdded},
            {"-", entry.LinesDeleted}
            
          };
        });

        foreach (var ptc in patch)
        {
          Sdx.Context.Current.Debug.Log(ptc);
        }
        //foreach (var note in result.Commit.Notes)
        //{
        //  Sdx.Context.Current.Debug.Log(note.Message);
        //}

        Sdx.Context.Current.Debug.Log(display);
        //ShowCommit(result.Commit);

        //foreach(var commit in result.Commit.Parents)
        //{
        //  ShowCommit(commit);
        //}
      }

      return View();
    }

    private void ShowCommit(LibGit2Sharp.Commit commit)
    {
      Sdx.Context.Current.Debug.Log(new Dictionary<string, object> { 
        {"ID", commit.Id},
        {"Message", commit.Message},
        {"Files", commit.Tree.Select(tree => tree.Path)}
      });
    }
  }
}