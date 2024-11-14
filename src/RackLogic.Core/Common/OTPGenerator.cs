using System.Security.Cryptography;
using System.Text;

namespace RackLogic.Core.Common;
public class OTPGenerator
{
  public static string GenerateOTP(int length)
  {
    using (var rngCsp = RandomNumberGenerator.Create())
    {
      byte[] randomBytes = new byte[length];
      rngCsp.GetBytes(randomBytes);

      StringBuilder otp = new StringBuilder();
      foreach (byte b in randomBytes)
      {
        otp.Append(b % 10);
      }

      return otp.ToString();
    }
  }
}
