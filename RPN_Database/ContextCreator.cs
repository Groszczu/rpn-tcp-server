using System.Data.Entity;

namespace RPN_Database
{
    /// <summary>
    /// Delegat tworzący kontekst bazy danych.
    /// </summary>
    /// <typeparam name="T">Klasa kontekstu.</typeparam>
    /// <returns>Utworzony kontekst.</returns>
    public delegate T ContextCreator<out T>() where T : DbContext;
}
