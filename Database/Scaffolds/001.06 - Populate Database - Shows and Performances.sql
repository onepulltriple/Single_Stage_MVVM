USE Single_Stage_MVVM;
GO

/*
    September 2026 test data.

    Assumes Show and Performance are empty.

    30 shows are created, spread throughout September.
    Performance rows are then created by matching Show.Name.
*/

BEGIN TRANSACTION;

BEGIN TRY

    -------------------------------------------------------------------------
    -- SHOWS
    -------------------------------------------------------------------------

    DECLARE @Shows TABLE
    (
        Name varchar(50) NOT NULL,
        StartTime datetime NOT NULL,
        EndTime datetime NOT NULL,
        TicketPrice decimal NOT NULL
    );

    INSERT INTO @Shows (Name, StartTime, EndTime, TicketPrice)
    VALUES

    -- Tuesday 1 September
    ('Tuesday Jazz Session',
        '2026-09-01 19:00', '2026-09-01 22:30', 18),

    -- Wednesday 2 September
    ('Wednesday Comedy Night',
        '2026-09-02 19:00', '2026-09-02 22:00', 20),

    -- Thursday 3 September
    ('Thursday Busker Showcase',
        '2026-09-03 17:00', '2026-09-03 21:00', 10),

    -- Friday 4 September
    ('Friday Rock Explosion',
        '2026-09-04 19:00', '2026-09-04 23:00', 35),

    -- Saturday 5 September
    ('Saturday Night Live Music',
        '2026-09-05 18:00', '2026-09-05 23:00', 32),

    -- Sunday 6 September
    ('Sunday Family Matinee',
        '2026-09-06 11:00', '2026-09-06 15:00', 12),

    -- Tuesday 8 September
    ('Autumn Jazz Evening',
        '2026-09-08 19:00', '2026-09-08 22:30', 22),

    -- Wednesday 9 September
    ('Comedy Under the Lights',
        '2026-09-09 19:30', '2026-09-09 22:30', 22),

    -- Thursday 10 September
    ('Midweek Burlesque',
        '2026-09-10 20:00', '2026-09-10 23:00', 28),

    -- Friday 11 September
    ('Friday Indie Showcase',
        '2026-09-11 19:00', '2026-09-11 23:00', 25),

    -- Saturday 12 September
    ('Saturday Rock Festival',
        '2026-09-12 17:00', '2026-09-12 23:30', 40),

    -- Sunday 13 September
    ('Sunday Acoustic Afternoon',
        '2026-09-13 13:00', '2026-09-13 17:00', 15),

    -- Tuesday 15 September
    ('Late Summer Jazz',
        '2026-09-15 18:30', '2026-09-15 22:00', 20),

    -- Wednesday 16 September
    ('Wednesday Laughs',
        '2026-09-16 19:00', '2026-09-16 22:00', 18),

    -- Thursday 17 September
    ('Buskers and Street Sounds',
        '2026-09-17 16:00', '2026-09-17 20:00', 10),

    -- Friday 18 September
    ('Friday Night Rock',
        '2026-09-18 19:00', '2026-09-18 23:00', 35),

    -- Saturday 19 September
    ('Saturday Comedy Special',
        '2026-09-19 19:00', '2026-09-19 22:30', 30),

    -- Sunday 20 September
    ('Sunday Classical Afternoon',
        '2026-09-20 14:00', '2026-09-20 17:30', 25),

    -- Tuesday 22 September
    ('Tuesday Blue Note Night',
        '2026-09-22 19:00', '2026-09-22 22:30', 22),

    -- Wednesday 23 September
    ('Comedy Night: September Special',
        '2026-09-23 19:00', '2026-09-23 22:30', 24),

    -- Thursday 24 September
    ('Burlesque After Dark',
        '2026-09-24 20:00', '2026-09-24 23:30', 32),

    -- Friday 25 September
    ('Friday Alternative Rock',
        '2026-09-25 19:00', '2026-09-25 23:00', 30),

    -- Saturday 26 September
    ('Big Saturday Concert',
        '2026-09-26 18:00', '2026-09-26 23:30', 45),

    -- Sunday 27 September
    ('Sunday Busker Brunch',
        '2026-09-27 10:00', '2026-09-27 14:00', 12),

    -- Monday 28 September
    ('Monday Music Showcase',
        '2026-09-28 18:00', '2026-09-28 21:30', 15),

    -- Tuesday 29 September
    ('End of Month Jazz',
        '2026-09-29 19:00', '2026-09-29 22:30', 22),

    -- Wednesday 30 September
    ('Final Wednesday Comedy',
        '2026-09-30 19:00', '2026-09-30 22:00', 20),

    -- Additional weekend events
    ('September Sunday Songwriters',
        '2026-09-06 16:00', '2026-09-06 20:00', 16),

    ('Saturday Afternoon Sessions',
        '2026-09-19 14:00', '2026-09-19 18:00', 14),

    ('September Closing Concert',
        '2026-09-30 14:00', '2026-09-30 18:00', 18);


    INSERT INTO Show (Name, StartTime, EndTime, TicketPrice)
    SELECT Name, StartTime, EndTime, TicketPrice
    FROM @Shows;


    -------------------------------------------------------------------------
    -- PERFORMANCES
    -------------------------------------------------------------------------

    DECLARE @Performances TABLE
    (
        ShowName varchar(50) NOT NULL,
        Description varchar(100) NOT NULL,
        StartTime datetime NOT NULL,
        EndTime datetime NOT NULL
    );

    INSERT INTO @Performances (ShowName, Description, StartTime, EndTime)
    VALUES

    -------------------------------------------------------------------------
    -- Tuesday Jazz Session
    -------------------------------------------------------------------------
    ('Tuesday Jazz Session', 'Opening Jazz Set',
        '2026-09-01 19:00', '2026-09-01 19:40'),
    ('Tuesday Jazz Session', 'Jazz Trio',
        '2026-09-01 19:50', '2026-09-01 20:30'),
    ('Tuesday Jazz Session', 'Guest Jazz Quartet',
        '2026-09-01 20:40', '2026-09-01 21:30'),
    ('Tuesday Jazz Session', 'Late Jazz Set',
        '2026-09-01 21:40', '2026-09-01 22:20'),

    -------------------------------------------------------------------------
    -- Wednesday Comedy Night
    -------------------------------------------------------------------------
    ('Wednesday Comedy Night', 'Opening Comedian',
        '2026-09-02 19:00', '2026-09-02 19:30'),
    ('Wednesday Comedy Night', 'Stand-Up Set 1',
        '2026-09-02 19:40', '2026-09-02 20:15'),
    ('Wednesday Comedy Night', 'Stand-Up Set 2',
        '2026-09-02 20:25', '2026-09-02 21:00'),
    ('Wednesday Comedy Night', 'Headliner',
        '2026-09-02 21:10', '2026-09-02 21:55'),

    -------------------------------------------------------------------------
    -- Thursday Busker Showcase
    -------------------------------------------------------------------------
    ('Thursday Busker Showcase', 'Busker 1',
        '2026-09-03 17:00', '2026-09-03 17:40'),
    ('Thursday Busker Showcase', 'Busker 2',
        '2026-09-03 17:50', '2026-09-03 18:30'),
    ('Thursday Busker Showcase', 'Busker 3',
        '2026-09-03 18:40', '2026-09-03 19:20'),
    ('Thursday Busker Showcase', 'Busker 4',
        '2026-09-03 19:30', '2026-09-03 20:10'),
    ('Thursday Busker Showcase', 'Busker Finale',
        '2026-09-03 20:20', '2026-09-03 20:50'),

    -------------------------------------------------------------------------
    -- Friday Rock Explosion
    -------------------------------------------------------------------------
    ('Friday Rock Explosion', 'Opening Band 1',
        '2026-09-04 19:00', '2026-09-04 19:45'),
    ('Friday Rock Explosion', 'Opening Band 2',
        '2026-09-04 20:00', '2026-09-04 20:45'),
    ('Friday Rock Explosion', 'Headliner',
        '2026-09-04 21:15', '2026-09-04 22:45'),

    -------------------------------------------------------------------------
    -- Saturday Night Live Music
    -------------------------------------------------------------------------
    ('Saturday Night Live Music', 'Opening Band 1',
        '2026-09-05 18:00', '2026-09-05 18:45'),
    ('Saturday Night Live Music', 'Opening Band 2',
        '2026-09-05 19:00', '2026-09-05 19:45'),
    ('Saturday Night Live Music', 'Special Guest',
        '2026-09-05 20:00', '2026-09-05 20:45'),
    ('Saturday Night Live Music', 'Headliner',
        '2026-09-05 21:15', '2026-09-05 22:45'),

    -------------------------------------------------------------------------
    -- Sunday Family Matinee
    -------------------------------------------------------------------------
    ('Sunday Family Matinee', 'Family Music Show',
        '2026-09-06 11:00', '2026-09-06 11:50'),
    ('Sunday Family Matinee', 'Comedy for Families',
        '2026-09-06 12:00', '2026-09-06 12:40'),
    ('Sunday Family Matinee', 'Magic and Variety',
        '2026-09-06 12:50', '2026-09-06 13:30'),
    ('Sunday Family Matinee', 'Finale',
        '2026-09-06 13:40', '2026-09-06 14:30'),

    -------------------------------------------------------------------------
    -- Autumn Jazz Evening
    -------------------------------------------------------------------------
    ('Autumn Jazz Evening', 'Opening Jazz Set',
        '2026-09-08 19:00', '2026-09-08 19:40'),
    ('Autumn Jazz Evening', 'Jazz Quartet',
        '2026-09-08 19:50', '2026-09-08 20:35'),
    ('Autumn Jazz Evening', 'Guest Saxophone Set',
        '2026-09-08 20:45', '2026-09-08 21:25'),
    ('Autumn Jazz Evening', 'Closing Jazz Set',
        '2026-09-08 21:35', '2026-09-08 22:15'),

    -------------------------------------------------------------------------
    -- Comedy Under the Lights
    -------------------------------------------------------------------------
    ('Comedy Under the Lights', 'Opening Comedian',
        '2026-09-09 19:30', '2026-09-09 20:00'),
    ('Comedy Under the Lights', 'Stand-Up Set',
        '2026-09-09 20:10', '2026-09-09 20:45'),
    ('Comedy Under the Lights', 'Guest Comedian',
        '2026-09-09 20:55', '2026-09-09 21:30'),
    ('Comedy Under the Lights', 'Headliner',
        '2026-09-09 21:40', '2026-09-09 22:20'),

    -------------------------------------------------------------------------
    -- Midweek Burlesque
    -------------------------------------------------------------------------
    ('Midweek Burlesque', 'Opening Variety Act',
        '2026-09-10 20:00', '2026-09-10 20:30'),
    ('Midweek Burlesque', 'Burlesque Act 1',
        '2026-09-10 20:40', '2026-09-10 21:10'),
    ('Midweek Burlesque', 'Burlesque Act 2',
        '2026-09-10 21:20', '2026-09-10 21:50'),
    ('Midweek Burlesque', 'Burlesque Finale',
        '2026-09-10 22:00', '2026-09-10 22:40'),

    -------------------------------------------------------------------------
    -- Friday Indie Showcase
    -------------------------------------------------------------------------
    ('Friday Indie Showcase', 'Opening Band 1',
        '2026-09-11 19:00', '2026-09-11 19:40'),
    ('Friday Indie Showcase', 'Opening Band 2',
        '2026-09-11 19:50', '2026-09-11 20:30'),
    ('Friday Indie Showcase', 'Indie Guest',
        '2026-09-11 20:40', '2026-09-11 21:20'),
    ('Friday Indie Showcase', 'Headliner',
        '2026-09-11 21:30', '2026-09-11 22:40'),

    -------------------------------------------------------------------------
    -- Saturday Rock Festival
    -------------------------------------------------------------------------
    ('Saturday Rock Festival', 'Opening Band 1',
        '2026-09-12 17:00', '2026-09-12 17:45'),
    ('Saturday Rock Festival', 'Opening Band 2',
        '2026-09-12 18:00', '2026-09-12 18:45'),
    ('Saturday Rock Festival', 'Special Guest',
        '2026-09-12 19:00', '2026-09-12 19:50'),
    ('Saturday Rock Festival', 'Headliner',
        '2026-09-12 20:15', '2026-09-12 22:00'),
    ('Saturday Rock Festival', 'Encore',
        '2026-09-12 22:15', '2026-09-12 22:45'),

    -------------------------------------------------------------------------
    -- Sunday Acoustic Afternoon
    -------------------------------------------------------------------------
    ('Sunday Acoustic Afternoon', 'Songwriter Set 1',
        '2026-09-13 13:00', '2026-09-13 13:45'),
    ('Sunday Acoustic Afternoon', 'Acoustic Duo',
        '2026-09-13 14:00', '2026-09-13 14:45'),
    ('Sunday Acoustic Afternoon', 'Songwriter Set 2',
        '2026-09-13 15:00', '2026-09-13 15:45'),
    ('Sunday Acoustic Afternoon', 'Acoustic Finale',
        '2026-09-13 16:00', '2026-09-13 16:45'),

    -------------------------------------------------------------------------
    -- Late Summer Jazz
    -------------------------------------------------------------------------
    ('Late Summer Jazz', 'Opening Jazz Set',
        '2026-09-15 18:30', '2026-09-15 19:10'),
    ('Late Summer Jazz', 'Jazz Trio',
        '2026-09-15 19:20', '2026-09-15 20:00'),
    ('Late Summer Jazz', 'Guest Quartet',
        '2026-09-15 20:10', '2026-09-15 20:55'),
    ('Late Summer Jazz', 'Closing Set',
        '2026-09-15 21:05', '2026-09-15 21:50'),

    -------------------------------------------------------------------------
    -- Wednesday Laughs
    -------------------------------------------------------------------------
    ('Wednesday Laughs', 'Opening Comedian',
        '2026-09-16 19:00', '2026-09-16 19:30'),
    ('Wednesday Laughs', 'Stand-Up Set 1',
        '2026-09-16 19:40', '2026-09-16 20:15'),
    ('Wednesday Laughs', 'Stand-Up Set 2',
        '2026-09-16 20:25', '2026-09-16 21:00'),
    ('Wednesday Laughs', 'Headliner',
        '2026-09-16 21:10', '2026-09-16 21:50'),

    -------------------------------------------------------------------------
    -- Buskers and Street Sounds
    -------------------------------------------------------------------------
    ('Buskers and Street Sounds', 'Busker 1',
        '2026-09-17 16:00', '2026-09-17 16:40'),
    ('Buskers and Street Sounds', 'Busker 2',
        '2026-09-17 16:50', '2026-09-17 17:30'),
    ('Buskers and Street Sounds', 'Busker 3',
        '2026-09-17 17:40', '2026-09-17 18:20'),
    ('Buskers and Street Sounds', 'Busker 4',
        '2026-09-17 18:30', '2026-09-17 19:10'),
    ('Buskers and Street Sounds', 'Busker Finale',
        '2026-09-17 19:20', '2026-09-17 19:50'),

    -------------------------------------------------------------------------
    -- Friday Night Rock
    -------------------------------------------------------------------------
    ('Friday Night Rock', 'Opening Band 1',
        '2026-09-18 19:00', '2026-09-18 19:45'),
    ('Friday Night Rock', 'Opening Band 2',
        '2026-09-18 20:00', '2026-09-18 20:45'),
    ('Friday Night Rock', 'Headliner',
        '2026-09-18 21:15', '2026-09-18 22:45'),

    -------------------------------------------------------------------------
    -- Saturday Comedy Special
    -------------------------------------------------------------------------
    ('Saturday Comedy Special', 'Opening Comedian',
        '2026-09-19 19:00', '2026-09-19 19:35'),
    ('Saturday Comedy Special', 'Guest Comedian 1',
        '2026-09-19 19:45', '2026-09-19 20:20'),
    ('Saturday Comedy Special', 'Guest Comedian 2',
        '2026-09-19 20:30', '2026-09-19 21:05'),
    ('Saturday Comedy Special', 'Headliner',
        '2026-09-19 21:15', '2026-09-19 22:10'),

    -------------------------------------------------------------------------
    -- Sunday Classical Afternoon
    -------------------------------------------------------------------------
    ('Sunday Classical Afternoon', 'String Quartet',
        '2026-09-20 14:00', '2026-09-20 14:45'),
    ('Sunday Classical Afternoon', 'Piano Recital',
        '2026-09-20 14:55', '2026-09-20 15:35'),
    ('Sunday Classical Afternoon', 'Chamber Ensemble',
        '2026-09-20 15:45', '2026-09-20 16:30'),
    ('Sunday Classical Afternoon', 'Closing Performance',
        '2026-09-20 16:40', '2026-09-20 17:15'),

    -------------------------------------------------------------------------
    -- Tuesday Blue Note Night
    -------------------------------------------------------------------------
    ('Tuesday Blue Note Night', 'Opening Jazz Set',
        '2026-09-22 19:00', '2026-09-22 19:40'),
    ('Tuesday Blue Note Night', 'Blue Note Trio',
        '2026-09-22 19:50', '2026-09-22 20:35'),
    ('Tuesday Blue Note Night', 'Guest Trumpet Set',
        '2026-09-22 20:45', '2026-09-22 21:25'),
    ('Tuesday Blue Note Night', 'Late Jazz Set',
        '2026-09-22 21:35', '2026-09-22 22:15'),

    -------------------------------------------------------------------------
    -- Comedy Night: September Special
    -------------------------------------------------------------------------
    ('Comedy Night: September Special', 'Opening Comedian',
        '2026-09-23 19:00', '2026-09-23 19:35'),
    ('Comedy Night: September Special', 'Stand-Up Set 1',
        '2026-09-23 19:45', '2026-09-23 20:20'),
    ('Comedy Night: September Special', 'Stand-Up Set 2',
        '2026-09-23 20:30', '2026-09-23 21:05'),
    ('Comedy Night: September Special', 'Headliner',
        '2026-09-23 21:15', '2026-09-23 22:15'),

    -------------------------------------------------------------------------
    -- Burlesque After Dark
    -------------------------------------------------------------------------
    ('Burlesque After Dark', 'Opening Variety Act',
        '2026-09-24 20:00', '2026-09-24 20:30'),
    ('Burlesque After Dark', 'Burlesque Act 1',
        '2026-09-24 20:40', '2026-09-24 21:10'),
    ('Burlesque After Dark', 'Burlesque Act 2',
        '2026-09-24 21:20', '2026-09-24 21:50'),
    ('Burlesque After Dark', 'Burlesque Act 3',
        '2026-09-24 22:00', '2026-09-24 22:30'),
    ('Burlesque After Dark', 'Grand Finale',
        '2026-09-24 22:40', '2026-09-24 23:15'),

    -------------------------------------------------------------------------
    -- Friday Alternative Rock
    -------------------------------------------------------------------------
    ('Friday Alternative Rock', 'Opening Band 1',
        '2026-09-25 19:00', '2026-09-25 19:45'),
    ('Friday Alternative Rock', 'Opening Band 2',
        '2026-09-25 20:00', '2026-09-25 20:45'),
    ('Friday Alternative Rock', 'Headliner',
        '2026-09-25 21:15', '2026-09-25 22:45'),

    -------------------------------------------------------------------------
    -- Big Saturday Concert
    -------------------------------------------------------------------------
    ('Big Saturday Concert', 'Opening Band 1',
        '2026-09-26 18:00', '2026-09-26 18:45'),
    ('Big Saturday Concert', 'Opening Band 2',
        '2026-09-26 19:00', '2026-09-26 19:45'),
    ('Big Saturday Concert', 'Special Guest',
        '2026-09-26 20:00', '2026-09-26 20:50'),
    ('Big Saturday Concert', 'Headliner',
        '2026-09-26 21:15', '2026-09-26 23:00'),

    -------------------------------------------------------------------------
    -- Sunday Busker Brunch
    -------------------------------------------------------------------------
    ('Sunday Busker Brunch', 'Busker 1',
        '2026-09-27 10:00', '2026-09-27 10:40'),
    ('Sunday Busker Brunch', 'Busker 2',
        '2026-09-27 10:50', '2026-09-27 11:30'),
    ('Sunday Busker Brunch', 'Busker 3',
        '2026-09-27 11:40', '2026-09-27 12:20'),
    ('Sunday Busker Brunch', 'Busker 4',
        '2026-09-27 12:30', '2026-09-27 13:10'),

    -------------------------------------------------------------------------
    -- Monday Music Showcase
    -------------------------------------------------------------------------
    ('Monday Music Showcase', 'Opening Act',
        '2026-09-28 18:00', '2026-09-28 18:40'),
    ('Monday Music Showcase', 'Acoustic Act',
        '2026-09-28 18:50', '2026-09-28 19:30'),
    ('Monday Music Showcase', 'Indie Act',
        '2026-09-28 19:40', '2026-09-28 20:20'),
    ('Monday Music Showcase', 'Closing Act',
        '2026-09-28 20:30', '2026-09-28 21:15'),

    -------------------------------------------------------------------------
    -- End of Month Jazz
    -------------------------------------------------------------------------
    ('End of Month Jazz', 'Opening Jazz Set',
        '2026-09-29 19:00', '2026-09-29 19:40'),
    ('End of Month Jazz', 'Jazz Trio',
        '2026-09-29 19:50', '2026-09-29 20:35'),
    ('End of Month Jazz', 'Guest Quartet',
        '2026-09-29 20:45', '2026-09-29 21:25'),
    ('End of Month Jazz', 'Closing Jazz Set',
        '2026-09-29 21:35', '2026-09-29 22:15'),

    -------------------------------------------------------------------------
    -- Final Wednesday Comedy
    -------------------------------------------------------------------------
    ('Final Wednesday Comedy', 'Opening Comedian',
        '2026-09-30 19:00', '2026-09-30 19:30'),
    ('Final Wednesday Comedy', 'Stand-Up Set 1',
        '2026-09-30 19:40', '2026-09-30 20:15'),
    ('Final Wednesday Comedy', 'Stand-Up Set 2',
        '2026-09-30 20:25', '2026-09-30 21:00'),
    ('Final Wednesday Comedy', 'Headliner',
        '2026-09-30 21:10', '2026-09-30 21:50'),

    -------------------------------------------------------------------------
    -- September Sunday Songwriters
    -------------------------------------------------------------------------
    ('September Sunday Songwriters', 'Songwriter 1',
        '2026-09-06 16:00', '2026-09-06 16:45'),
    ('September Sunday Songwriters', 'Songwriter 2',
        '2026-09-06 16:55', '2026-09-06 17:40'),
    ('September Sunday Songwriters', 'Songwriter 3',
        '2026-09-06 17:50', '2026-09-06 18:35'),
    ('September Sunday Songwriters', 'Songwriter Finale',
        '2026-09-06 18:45', '2026-09-06 19:30'),

    -------------------------------------------------------------------------
    -- Saturday Afternoon Sessions
    -------------------------------------------------------------------------
    ('Saturday Afternoon Sessions', 'Acoustic Duo',
        '2026-09-19 14:00', '2026-09-19 14:45'),
    ('Saturday Afternoon Sessions', 'Singer-Songwriter',
        '2026-09-19 14:55', '2026-09-19 15:40'),
    ('Saturday Afternoon Sessions', 'Folk Trio',
        '2026-09-19 15:50', '2026-09-19 16:35'),
    ('Saturday Afternoon Sessions', 'Closing Act',
        '2026-09-19 16:45', '2026-09-19 17:30'),

    -------------------------------------------------------------------------
    -- September Closing Concert
    -------------------------------------------------------------------------
    ('September Closing Concert', 'Opening Act 1',
        '2026-09-30 14:00', '2026-09-30 14:40'),
    ('September Closing Concert', 'Opening Act 2',
        '2026-09-30 14:50', '2026-09-30 15:30'),
    ('September Closing Concert', 'Guest Performance',
        '2026-09-30 15:40', '2026-09-30 16:20'),
    ('September Closing Concert', 'Closing Performance',
        '2026-09-30 16:30', '2026-09-30 17:30');


    -------------------------------------------------------------------------
    -- INSERT PERFORMANCES
    --
    -- Resolve Show_id from Show.Name rather than relying on identity values.
    -------------------------------------------------------------------------

    INSERT INTO Performance
    (
        Description,
        StartTime,
        EndTime,
        Show_id
    )
    SELECT
        p.Description,
        p.StartTime,
        p.EndTime,
        s.id
    FROM @Performances p
    INNER JOIN Show s
        ON s.Name = p.ShowName;


    -------------------------------------------------------------------------
    -- BASIC VERIFICATION
    -------------------------------------------------------------------------

    SELECT
        s.id,
        s.Name,
        s.StartTime,
        s.EndTime,
        s.TicketPrice,
        COUNT(p.id) AS PerformanceCount
    FROM Show s
    LEFT JOIN Performance p
        ON p.Show_id = s.id
    GROUP BY
        s.id,
        s.Name,
        s.StartTime,
        s.EndTime,
        s.TicketPrice
    ORDER BY
        s.StartTime;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO
