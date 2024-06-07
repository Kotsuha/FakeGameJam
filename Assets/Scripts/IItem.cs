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
    void DeBuff(IPlayer player);
}

public interface IBuff
{
    void Buff(IPlayer player);
}

public interface IPlayer
{

}