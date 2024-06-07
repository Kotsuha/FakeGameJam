using System;

public interface IItem
{
    event Action<string> OnGetItem;
    string Name { get; }
}


public interface IBuffItem : IItem, IBuff
{

}


public interface IDebuffItem : IItem, IDebuff
{

}




public interface IDebuff
{
    void DeBuff();
}

public interface IBuff
{
    void Buff();
}