namespace TennisTournament.Application.DTOs;

public class MatchRequestDto
{
  public required PlayerDto Player1 { get; set; }
  public required PlayerDto Player2 { get; set; }
}
