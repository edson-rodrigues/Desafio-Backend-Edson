using Mottu.Domain.Common;
using Mottu.Domain.Enums;
using Mottu.Domain.Exceptions;
using Mottu.Domain.ValueObjects;

namespace Mottu.Domain.Entities;

public class Rental : Entity
{
    public Guid MotorcycleId { get; private set; }
    public Guid DeliveryDriverId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime ExpectedEndDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }
    public RentalPlan Plan { get; private set; }
    public decimal TotalCost { get; private set; }
    public RentalStatus Status { get; private set; }

    // Navigation properties
    public virtual Motorcycle? Motorcycle { get; private set; }
    public virtual DeliveryDriver? DeliveryDriver { get; private set; }

    private Rental() : base() { }

    private Rental(
        Guid id,
        Guid motorcycleId,
        Guid deliveryDriverId,
        DateTime startDate,
        DateTime endDate,
        DateTime expectedEndDate,
        RentalPlan plan) : base(id)
    {
        MotorcycleId = motorcycleId;
        DeliveryDriverId = deliveryDriverId;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
        Plan = plan;
        TotalCost = plan.TotalCost;
        Status = RentalStatus.Active;
    }

    public static Rental Create(
        Guid motorcycleId,
        Guid deliveryDriverId,
        int planDurationDays,
        DateTime? requestDate = null)
    {
        if (motorcycleId == Guid.Empty)
            throw new DomainException("INVALID_MOTORCYCLE_ID", "Motorcycle ID cannot be empty");

        if (deliveryDriverId == Guid.Empty)
            throw new DomainException("INVALID_DELIVERY_DRIVER_ID", "Delivery driver ID cannot be empty");

        var plan = RentalPlan.Create(planDurationDays);
        var creationDate = requestDate ?? DateTime.UtcNow;
        
        // Start date is the first day after creation
        var startDate = creationDate.Date.AddDays(1);
        var expectedEndDate = startDate.AddDays(plan.DurationDays);
        var endDate = expectedEndDate;

        return new Rental(
            Guid.NewGuid(),
            motorcycleId,
            deliveryDriverId,
            startDate,
            endDate,
            expectedEndDate,
            plan);
    }

    public decimal CalculateReturnCost(DateTime returnDate)
    {
        if (returnDate < StartDate)
            throw new DomainException("INVALID_RETURN_DATE", "Return date cannot be before start date");

        if (Status != RentalStatus.Active)
            throw new DomainException("INVALID_RENTAL_STATUS", "Cannot calculate cost for non-active rental");

        var actualDays = (int)(returnDate.Date - StartDate.Date).TotalDays;
        var plannedDays = Plan.DurationDays;

        decimal totalCost = 0;

        if (actualDays < plannedDays)
        {
            // Early return - charge for used days plus penalty
            totalCost = actualDays * Plan.DailyCost;
            var unusedDays = plannedDays - actualDays;
            var penalty = Plan.CalculatePenalty(unusedDays);
            totalCost += penalty;
        }
        else if (actualDays > plannedDays)
        {
            // Late return - charge for all planned days plus late fees
            totalCost = Plan.TotalCost;
            var extraDays = actualDays - plannedDays;
            var lateFee = Plan.CalculateLateFee(extraDays);
            totalCost += lateFee;
        }
        else
        {
            // On time return
            totalCost = Plan.TotalCost;
        }

        return totalCost;
    }

    public void CompleteRental(DateTime returnDate)
    {
        if (Status != RentalStatus.Active)
            throw new DomainException("INVALID_RENTAL_STATUS", "Rental is not active");

        ActualReturnDate = returnDate;
        TotalCost = CalculateReturnCost(returnDate);
        Status = RentalStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != RentalStatus.Active)
            throw new DomainException("INVALID_RENTAL_STATUS", "Only active rentals can be cancelled");

        Status = RentalStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}

