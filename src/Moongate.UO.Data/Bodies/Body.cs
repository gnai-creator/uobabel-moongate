namespace Moongate.UO.Data.Bodies;

public struct Body
{
    public static BodyType[] Types;


    public Body(int bodyID)
    {
        BodyID = bodyID;
    }

    public BodyType Type
    {
        get
        {
            if (BodyID >= 0 && BodyID < Types.Length)
            {
                return Types[BodyID];
            }
            else
            {
                return BodyType.Empty;
            }
        }
    }

    public bool IsHuman
    {
        get
        {
            return (BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Human && BodyID != 402 &&
                    BodyID != 403 && BodyID != 607 && BodyID != 608 && BodyID != 970)

                   #region Stygian Abyss

                   || BodyID == 694 || BodyID == 695

                #endregion

                ;
        }
    }

    public bool IsMale
    {
        get
        {
            return BodyID == 183 || BodyID == 185 || BodyID == 400 || BodyID == 402 || BodyID == 605 ||
                   BodyID == 607 || BodyID == 750

                   #region Stygian Abyss

                   || BodyID == 666 || BodyID == 694

                #endregion

                ;
        }
    }

    public bool IsFemale
    {
        get
        {
            return BodyID == 184 || BodyID == 186 || BodyID == 401 || BodyID == 403 || BodyID == 606 ||
                   BodyID == 608 || BodyID == 751

                   #region Stygian Abyss

                   || BodyID == 667 || BodyID == 695

                   #endregion

                   #region High Seas

                   || BodyID == 1253

                #endregion

                ;
        }
    }

    public bool IsGhost
    {
        get
        {
            return BodyID == 402 || BodyID == 403 || BodyID == 607 || BodyID == 608 || BodyID == 970

                   #region Stygian Abyss

                   || BodyID == 694 || BodyID == 695

                #endregion

                ;
        }
    }

    public bool IsMonster => BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Monster;

    public bool IsAnimal => BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Animal;

    public bool IsEmpty => BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Empty;

    public bool IsSea => BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Sea;

    public bool IsEquipment => BodyID >= 0 && BodyID < Types.Length && Types[BodyID] == BodyType.Equipment;

    #region Stygian Abyss

    public bool IsGargoyle => BodyID == 666 || BodyID == 667 || BodyID == 694 || BodyID == 695;

    #endregion

    public int BodyID { get; }

    public static implicit operator int(Body a)
    {
        return a.BodyID;
    }

    public static implicit operator Body(int a)
    {
        return new Body(a);
    }

    public override string ToString()
    {
        return $"0x{BodyID:X}";
    }

    public override int GetHashCode()
    {
        return BodyID;
    }

    public override bool Equals(object o)
    {
        if (!(o is Body))
        {
            return false;
        }

        return ((Body)o).BodyID == BodyID;
    }

    public static bool operator ==(Body l, Body r)
    {
        return l.BodyID == r.BodyID;
    }

    public static bool operator !=(Body l, Body r)
    {
        return l.BodyID != r.BodyID;
    }

    public static bool operator >(Body l, Body r)
    {
        return l.BodyID > r.BodyID;
    }

    public static bool operator >=(Body l, Body r)
    {
        return l.BodyID >= r.BodyID;
    }

    public static bool operator <(Body l, Body r)
    {
        return l.BodyID < r.BodyID;
    }

    public static bool operator <=(Body l, Body r)
    {
        return l.BodyID <= r.BodyID;
    }
}
