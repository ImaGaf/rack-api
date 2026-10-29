namespace RackLogic.Web.Configurations;

public static class DotEnvConfigs
{
  // Loads KEY=VALUE pairs from the nearest .env file (searching upward from the current directory)
  // into environment variables. Existing environment variables are not overwritten.
  // Use "__" for nested keys, e.g. SendGrid__ApiKey -> SendGrid:ApiKey.
  public static void Load(string fileName = ".env")
  {
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null && !File.Exists(Path.Combine(dir.FullName, fileName)))
    {
      dir = dir.Parent;
    }
    if (dir == null) return;

    foreach (var rawLine in File.ReadAllLines(Path.Combine(dir.FullName, fileName)))
    {
      var line = rawLine.Trim();
      if (line.Length == 0 || line.StartsWith('#')) continue;

      var separator = line.IndexOf('=');
      if (separator <= 0) continue;

      var key = line[..separator].Trim();
      var value = line[(separator + 1)..].Trim();
      if (value.Length >= 2 && (value[0] == '"' || value[0] == '\'') && value[^1] == value[0])
      {
        value = value[1..^1];
      }

      if (Environment.GetEnvironmentVariable(key) == null)
      {
        Environment.SetEnvironmentVariable(key, value);
      }
    }
  }
}
