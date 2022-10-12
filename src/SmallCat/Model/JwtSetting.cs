namespace SmallCat.Model;

public class JwtSetting
{
    /// <summary>
    /// 为IssuerSigningKeyByteArray的参数
    /// </summary>
    public string? IssuerSigningKey { get; set; }
    public byte[] IssuerSigningKeyByteArray => System.Text.Encoding.Default.GetBytes(IssuerSigningKey);

    /// <summary>
    /// 验证签发的Key
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; }
    /// <summary>
    /// 是否需要过期时间
    /// </summary>
    public bool RequireExpirationTime { get; set; }
    /// <summary>
    /// 是否验证发布者
    /// </summary>
    public bool ValidateIssuer { get; set; }
    /// <summary>
    /// 发布者名称
    /// </summary>
    public string ValidIssuer { get; set; }

    /// <summary>
    /// 是否验证订阅者
    /// </summary>
    public bool ValidateAudience { get; set; }
    /// <summary>
    /// 订阅者名称
    /// </summary>
    public string ValidAudience { get; set; }

    /// <summary>
    /// 验证令牌有效期
    /// </summary>
    public bool ValidateLifetime { get; set; }
    /// <summary>
    /// 每次颁发令牌，令牌有效时间(S)
    /// </summary>
    public int ClockSkew { get; set; }
}
