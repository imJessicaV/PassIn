//criar nossas proprias excessões

namespace PassIn.Exceptions;
public class PassInException : SystemException //Fazendo uma herança
{
    public PassInException(string message) : base(message) //pega amensagem recebida e passa para o construtor
    {
        
    }
}
