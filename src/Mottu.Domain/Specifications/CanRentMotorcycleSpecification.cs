using Mottu.Domain.Entities;
using Mottu.Domain.Enums;

namespace Mottu.Domain.Specifications;

public class CanRentMotorcycleSpecification
{
    public bool IsSatisfiedBy(DeliveryDriver driver)
    {
        if (driver == null)
            return false;

        return driver.CNH.Type == CNHType.A || driver.CNH.Type == CNHType.AB;
    }

    public string GetFailureReason(DeliveryDriver driver)
    {
        if (driver == null)
            return "Driver not found";

        if (driver.CNH.Type != CNHType.A && driver.CNH.Type != CNHType.AB)
            return "Driver must have CNH type A or AB to rent a motorcycle";

        return string.Empty;
    }
}

