namespace JukeboxProofOfConcept;

public class Melody((Tone, Note)[] tune, int bpm)
{
    public (Tone, Note)[] Tune { get; set; } = tune;
    public int Bpm { get; set; } = bpm;
    public int Mpsb => 1000 / (Bpm / 60) / 4; // Mps: Milliseconds per sixteenth beat

    public static Melody GetTetrisA(int bpm = 140)
    {
        (Tone, Note)[] tune =
        [
            (Tone.E5, Note.Quarter),
            (Tone.B4, Note.Eighth),
            (Tone.C5, Note.Eighth),
            (Tone.D5, Note.Quarter),
            (Tone.C5, Note.Eighth),
            (Tone.B4, Note.Eighth),

            (Tone.A4, Note.Quarter),
            (Tone.A4, Note.Eighth),
            (Tone.C5, Note.Eighth),
            (Tone.E5, Note.Quarter),
            (Tone.D5, Note.Eighth),
            (Tone.C5, Note.Eighth),

            (Tone.B4, Note.QuarterHalf),
            (Tone.C5, Note.Eighth),
            (Tone.D5, Note.Quarter),
            (Tone.E5, Note.Quarter),

            (Tone.C5, Note.Quarter),
            (Tone.A4, Note.Quarter),
            (Tone.A4, Note.Quarter),
            (Tone.QuarterRest, Note.Quarter),


            (Tone.EighthRest, Note.Eighth),
            (Tone.D5, Note.Quarter),
            (Tone.F5, Note.Eighth),
            (Tone.A5, Note.Quarter),
            (Tone.G5, Note.Eighth),
            (Tone.F5, Note.Eighth),

            (Tone.E5, Note.QuarterHalf),
            (Tone.C5, Note.Eighth),
            (Tone.E5, Note.Quarter),
            (Tone.D5, Note.Eighth),
            (Tone.C5, Note.Eighth),

            (Tone.B4, Note.QuarterHalf),
            (Tone.C5, Note.Eighth),
            (Tone.D5, Note.Quarter),
            (Tone.E5, Note.Quarter),

            (Tone.C5, Note.Quarter),
            (Tone.A4, Note.Quarter),
            (Tone.A4, Note.Quarter),
            (Tone.QuarterRest, Note.Quarter),


            (Tone.E5, Note.Half),
            (Tone.C5, Note.Half),

            (Tone.D5, Note.Half),
            (Tone.B4, Note.Half),

            (Tone.C5, Note.Half),
            (Tone.A4, Note.Half),

            (Tone.Gs4, Note.Whole),

            (Tone.E5, Note.Half),
            (Tone.C5, Note.Half),

            (Tone.D5, Note.Half),
            (Tone.B4, Note.Half),

            (Tone.C5, Note.Quarter),
            (Tone.E5, Note.Quarter),
            (Tone.A5, Note.Quarter),
            (Tone.A5, Note.Quarter),

            (Tone.Gs5, Note.Whole),
        ];

        return new Melody(tune, bpm);
    }
}