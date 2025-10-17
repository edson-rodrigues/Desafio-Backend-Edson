namespace Mottu.Application.DTOs;

public class RentalDto
{
    public string? Identificador { get; set; }
    public string? Entregador_id { get; set; }
    public string? Moto_id { get; set; }
    public DateTime Data_inicio { get; set; }
    public DateTime Data_termino { get; set; }
    public DateTime Data_previsao_termino { get; set; }
    public int Plano { get; set; }
    public decimal Valor_diaria { get; set; }
}

public class RentalCostDto
{
    public decimal Valor_total { get; set; }
}

