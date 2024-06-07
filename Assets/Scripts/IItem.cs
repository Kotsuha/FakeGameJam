using System;

public interface IItem
{
    event Action<string> OnGetItem;
    string Name { get; }
}