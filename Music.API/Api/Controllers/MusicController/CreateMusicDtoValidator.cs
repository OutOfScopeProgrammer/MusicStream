using FluentValidation;

namespace Music.API.Api.Controllers.MusicController;

public class CreateMusicDtoValidator : AbstractValidator<CreateMusicDto>
{
    public CreateMusicDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(d => d.Title).MinimumLength(1)
        .MaximumLength(50);

        RuleFor(d => d.Description)
        .MinimumLength(1)
        .MaximumLength(150);
    }
}
