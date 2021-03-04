
namespace ConsoleApp1
{

    interface IBase : IBaser { }
    interface IBaser : IBasiest { }
    interface IBasiest : ISuperBasiest { }
    interface ISuperBasiest : ITheEnd { }
    interface ITheEnd { }

    interface I1 : IBase { }
    interface I2 : IBase { }
    interface I3 : IBase { }
    interface I4 : IBase { }
    interface I5 : IBase { }
    interface I6 : IBase { }
    interface I7 : IBase { }
    interface I8 : IBase { }
    interface I9 : IBase { }
    interface I10 : IBase{ }
    interface I11 : IBase{ }
    interface I12 : IBase{ }
    interface I13 : IBase{ }
    interface I14 : IBase{ }
    interface I15 : IBase{ }
    interface I16 : IBase{ }
    interface I17 : IBase{ }
    interface I18 : IBase{ }
    interface I19 : IBase{ }
    interface I20 : IBase{ }

    public class MyMessage : 
        I1,
        I2,
        I3,
        I4,
        I5,
        I6,
        I7,
        I8,
        I9,
        I10,
        I11,
        I12,
        I13,
        I14,
        I15,
        I16,
        I17,
        I18,
        I19,
        I20
    {
    }
}
