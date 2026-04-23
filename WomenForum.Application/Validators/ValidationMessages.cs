namespace WomenForum.Validators;

public static class ValidationMessages
{
    public const string Required = "{PropertyName} обязательно для заполнения";
    public const string MaxLength = "{PropertyName} не должно превышать {MaxLength} символов";
    public const string MinLength = "{PropertyName} должно содержать минимум {MinLength} символов";
    public const string InvalidEmail = "Некорректный формат Email";
    public const string InvalidUsername = "{PropertyName} может содержать только буквы, цифры и _";
    public const string PasswordsNotMatch = "Пароли не совпадают";
    public const string InvalidBirthDate = "{PropertyName} должна быть в прошлом";
    public const string InvalidValue = "{PropertyName} неверное значение.";
}