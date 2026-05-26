using System.Security.Cryptography;

namespace UHabitacional.API.Application.Helpers;

/// <summary>
/// Generador de códigos aleatorios de 6 caracteres con:
/// al menos una mayúscula, una minúscula y un número.
/// </summary>
public static class CodigoVisitaGenerator
{
    private const string Mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Minusculas = "abcdefghjkmnpqrstuvwxyz";
    private const string Numeros = "23456789";
    private const string Todos = Mayusculas + Minusculas + Numeros;

    public static string Generar()
    {
        // Garantizar al menos 1 mayúscula, 1 minúscula y 1 número
        var chars = new char[6];
        chars[0] = Mayusculas[RandomNumberGenerator.GetInt32(Mayusculas.Length)];
        chars[1] = Minusculas[RandomNumberGenerator.GetInt32(Minusculas.Length)];
        chars[2] = Numeros[RandomNumberGenerator.GetInt32(Numeros.Length)];

        for (int i = 3; i < 6; i++)
            chars[i] = Todos[RandomNumberGenerator.GetInt32(Todos.Length)];

        // Mezclar el arreglo (Fisher-Yates)
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }
}
