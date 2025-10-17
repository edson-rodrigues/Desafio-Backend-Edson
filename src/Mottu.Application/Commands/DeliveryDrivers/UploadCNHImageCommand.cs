using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.DeliveryDrivers;

public record UploadCNHImageCommand(
    string DriverId,
    Stream ImageStream,
    string FileName,
    string ContentType
) : IRequest<Result>;

