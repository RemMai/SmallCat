namespace SmartCat.Model;

internal class JwtSetting
{
    public string? IssuerSigningKey { get; set; }
    public Byte[] IssuerSigningKeyByteArray { get => System.Text.Encoding.Default.GetBytes(IssuerSigningKey ?? "RemMai"); }

    public bool ValidateIssuerSigningKey { get; set; }

    public bool RequireExpirationTime { get; set; }
    public int ClockSkew { get; set; }
    public bool ValidateIssuer { get;  set; }
}
