USE Single_Stage_MVVM;
GO

/*
    SINGLE STAGE - TEST DATA
    September / October 2026

    Assumes the database has just been created and the tables are empty.

    Rules:
        - 50 shows
        - 11 September 2026 through 15 October 2026
        - Shows only between 10:00 and midnight
        - No show spans multiple days
        - Shows do not overlap
        - Each show has 2-5 performances
        - Performances stay inside their show's timeframe
        - Performances do not overlap
        - Gaps exist between performances
        - Some gaps are represented by an "Intermission & Host" performance
        - Each performance gets 1-3 artists
        - 25 ticketholders
        - Multiple tickets may be purchased by the same ticketholder
        - Ticket prices range from €15 to €180
        - Some shows are marked SoldOut independently of ticket sales
*/

SET NOCOUNT ON;

BEGIN TRANSACTION;

BEGIN TRY

    -------------------------------------------------------------------------
    -- SHOWS
    -------------------------------------------------------------------------

    DECLARE @Shows TABLE
    (
        ShowNo       int PRIMARY KEY,
        Name         varchar(50) NOT NULL,
        StartTime    datetime NOT NULL,
        EndTime      datetime NOT NULL,
        TicketPrice  decimal(18,0) NOT NULL,
        SoldOut      bit NOT NULL,
        Genre        varchar(20) NOT NULL,
        ShowId       int NULL
    );

    INSERT INTO @Shows
    (
        ShowNo,
        Name,
        StartTime,
        EndTime,
        TicketPrice,
        SoldOut,
        Genre
    )
    VALUES

    -------------------------------------------------------------------------
    -- SEPTEMBER 11
    -------------------------------------------------------------------------

    (1,
        'Friday Night Open Mic',
        '2026-09-11 21:15',
        '2026-09-11 23:30',
        18,
        0,
        'Variety'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 12
    -------------------------------------------------------------------------

    (2,
        'Saturday Morning Family Variety',
        '2026-09-12 10:30',
        '2026-09-12 13:00',
        15,
        0,
        'Family'),

    (3,
        'Saturday Rock Revue',
        '2026-09-12 19:00',
        '2026-09-12 23:00',
        95,
        1,
        'Rock'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 13
    -------------------------------------------------------------------------

    (4,
        'Sunday Jazz Brunch',
        '2026-09-13 11:00',
        '2026-09-13 14:00',
        42,
        0,
        'Jazz'),

    (5,
        'Sunday Comedy Club',
        '2026-09-13 18:00',
        '2026-09-13 21:00',
        35,
        1,
        'Comedy'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 14
    -------------------------------------------------------------------------

    (6,
        'Monday Songwriters Circle',
        '2026-09-14 18:30',
        '2026-09-14 21:00',
        22,
        0,
        'Singer'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 15
    -------------------------------------------------------------------------

    (7,
        'Morning Acoustic Cafe',
        '2026-09-15 10:30',
        '2026-09-15 12:30',
        18,
        0,
        'Singer'),

    (8,
        'Autumn Jazz Collective',
        '2026-09-15 19:00',
        '2026-09-15 22:30',
        55,
        1,
        'Jazz'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 16
    -------------------------------------------------------------------------

    (9,
        'Afternoon Classical Salon',
        '2026-09-16 14:00',
        '2026-09-16 17:00',
        65,
        0,
        'Classical'),

    (10,
        'Wednesday Laughs',
        '2026-09-16 19:30',
        '2026-09-16 22:30',
        28,
        0,
        'Comedy'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 17
    -------------------------------------------------------------------------

    (11,
        'Street Sounds Showcase',
        '2026-09-17 16:00',
        '2026-09-17 19:00',
        15,
        0,
        'Buskers'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 18
    -------------------------------------------------------------------------

    (12,
        'Friday Folk Matinee',
        '2026-09-18 11:00',
        '2026-09-18 14:00',
        20,
        0,
        'Singer'),

    (13,
        'Friday Rock Assembly',
        '2026-09-18 19:00',
        '2026-09-18 23:00',
        85,
        0,
        'Rock'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 19
    -------------------------------------------------------------------------

    (14,
        'Saturday Burlesque Tea',
        '2026-09-19 13:00',
        '2026-09-19 16:00',
        40,
        0,
        'Burlesque'),

    (15,
        'Velvet Burlesque Night',
        '2026-09-19 20:00',
        '2026-09-19 23:30',
        75,
        1,
        'Burlesque'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 20
    -------------------------------------------------------------------------

    (16,
        'Sunday Family Stage',
        '2026-09-20 10:30',
        '2026-09-20 13:30',
        18,
        0,
        'Family'),

    (17,
        'Indie Sunday Sessions',
        '2026-09-20 17:00',
        '2026-09-20 20:30',
        32,
        1,
        'Indie'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 21
    -------------------------------------------------------------------------

    (18,
        'Monday Blue Note',
        '2026-09-21 18:30',
        '2026-09-21 21:30',
        48,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 22
    -------------------------------------------------------------------------

    (19,
        'Lunchtime Piano Stories',
        '2026-09-22 12:00',
        '2026-09-22 15:00',
        30,
        0,
        'Classical'),

    (20,
        'Comedy Under the Lights',
        '2026-09-22 19:00',
        '2026-09-22 22:30',
        38,
        0,
        'Comedy'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 23
    -------------------------------------------------------------------------

    (21,
        'Singer Songwriter Social',
        '2026-09-23 15:00',
        '2026-09-23 18:00',
        25,
        0,
        'Singer'),

    (22,
        'Late Night Jazz',
        '2026-09-23 20:00',
        '2026-09-23 23:30',
        60,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 24
    -------------------------------------------------------------------------

    (23,
        'Morning Magic Matinee',
        '2026-09-24 10:00',
        '2026-09-24 12:30',
        22,
        0,
        'Variety'),

    (24,
        'Thursday Guitar Night',
        '2026-09-24 18:30',
        '2026-09-24 21:30',
        45,
        0,
        'Rock'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 25
    -------------------------------------------------------------------------

    (25,
        'Friday Busker Market',
        '2026-09-25 13:00',
        '2026-09-25 16:00',
        15,
        0,
        'Buskers'),

    (26,
        'Alternative Rock Friday',
        '2026-09-25 19:30',
        '2026-09-25 23:30',
        70,
        1,
        'Rock'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 26
    -------------------------------------------------------------------------

    (27,
        'Saturday Brunch Cabaret',
        '2026-09-26 11:00',
        '2026-09-26 14:00',
        55,
        0,
        'Variety'),

    (28,
        'Sunday Afternoon Songwriters',
        '2026-09-26 15:00',
        '2026-09-26 17:30',
        20,
        0,
        'Singer'),

    (29,
        'Big October Warmup',
        '2026-09-26 19:00',
        '2026-09-26 23:00',
        85,
        0,
        'Indie'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 27
    -------------------------------------------------------------------------

    (30,
        'Sunday Acoustic Matinee',
        '2026-09-27 12:00',
        '2026-09-27 15:00',
        18,
        0,
        'Singer'),

    (31,
        'Sunday Comedy Showcase',
        '2026-09-27 18:00',
        '2026-09-27 21:30',
        35,
        0,
        'Comedy'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 28
    -------------------------------------------------------------------------

    (32,
        'Monday Morning Music',
        '2026-09-28 10:30',
        '2026-09-28 13:00',
        16,
        0,
        'Singer'),

    (33,
        'Monday Mixed Bill',
        '2026-09-28 19:00',
        '2026-09-28 22:00',
        30,
        0,
        'Indie'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 29
    -------------------------------------------------------------------------

    (34,
        'Tuesday Chamber Afternoon',
        '2026-09-29 14:00',
        '2026-09-29 17:00',
        65,
        1,
        'Classical'),

    (35,
        'Tuesday Jazz Room',
        '2026-09-29 19:00',
        '2026-09-29 22:30',
        50,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- SEPTEMBER 30
    -------------------------------------------------------------------------

    (36,
        'Wednesday Childrens Theatre',
        '2026-09-30 11:00',
        '2026-09-30 14:00',
        20,
        0,
        'Family'),

    (37,
        'September Comedy Finale',
        '2026-09-30 19:00',
        '2026-09-30 22:30',
        40,
        0,
        'Comedy'),

    -------------------------------------------------------------------------
    -- OCTOBER 1
    -------------------------------------------------------------------------

    (38,
        'October Busker Evening',
        '2026-10-01 16:00',
        '2026-10-01 19:00',
        18,
        0,
        'Buskers'),

    (39,
        'October Jazz Launch',
        '2026-10-01 20:00',
        '2026-10-01 23:30',
        55,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- OCTOBER 2
    -------------------------------------------------------------------------

    (40,
        'Friday Acoustic Morning',
        '2026-10-02 10:30',
        '2026-10-02 13:30',
        17,
        0,
        'Singer'),

    (41,
        'Friday Indie Nights',
        '2026-10-02 19:00',
        '2026-10-02 23:00',
        45,
        0,
        'Indie'),

    -------------------------------------------------------------------------
    -- OCTOBER 3
    -------------------------------------------------------------------------

    (42,
        'Saturday Songwriter Brunch',
        '2026-10-03 12:00',
        '2026-10-03 15:00',
        25,
        0,
        'Singer'),

    (43,
        'Big October Rock Show',
        '2026-10-03 19:00',
        '2026-10-03 23:30',
        120,
        1,
        'Rock'),

    -------------------------------------------------------------------------
    -- OCTOBER 4
    -------------------------------------------------------------------------

    (44,
        'Sunday Family Matinee',
        '2026-10-04 11:00',
        '2026-10-04 14:00',
        18,
        0,
        'Family'),

    (45,
        'Sunday Jazz and Soul',
        '2026-10-04 17:30',
        '2026-10-04 20:30',
        45,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- OCTOBER 5
    -------------------------------------------------------------------------

    (46,
        'Monday Comedy Club',
        '2026-10-05 18:00',
        '2026-10-05 21:00',
        30,
        0,
        'Comedy'),

    -------------------------------------------------------------------------
    -- OCTOBER 6
    -------------------------------------------------------------------------

    (47,
        'Tuesday Folk Afternoon',
        '2026-10-06 13:00',
        '2026-10-06 16:00',
        22,
        0,
        'Singer'),

    -------------------------------------------------------------------------
    -- OCTOBER 7
    -------------------------------------------------------------------------

    (48,
        'Wednesday Variety Night',
        '2026-10-07 19:00',
        '2026-10-07 22:30',
        35,
        0,
        'Variety'),

    -------------------------------------------------------------------------
    -- OCTOBER 9
    -------------------------------------------------------------------------

    (49,
        'Friday Morning Jazz',
        '2026-10-09 11:00',
        '2026-10-09 14:00',
        20,
        0,
        'Jazz'),

    -------------------------------------------------------------------------
    -- OCTOBER 10
    -------------------------------------------------------------------------

    (50,
        'Saturday All Star Stage',
        '2026-10-10 18:30',
        '2026-10-10 23:30',
        150,
        1,
        'Variety');

    -------------------------------------------------------------------------
    -- INSERT SHOWS
    -------------------------------------------------------------------------

    INSERT INTO Show
    (
        Name,
        StartTime,
        EndTime,
        TicketPrice,
        SoldOut
    )
    SELECT
        Name,
        StartTime,
        EndTime,
        TicketPrice,
        SoldOut
    FROM @Shows;

    -------------------------------------------------------------------------
    -- RESOLVE GENERATED SHOW IDs
    -------------------------------------------------------------------------

    UPDATE seed
    SET seed.ShowId = s.id
    FROM @Shows seed
    INNER JOIN Show s
        ON s.Name = seed.Name;

    -------------------------------------------------------------------------
    -- ARTISTS
    --
    -- The Genre column only exists in the seed table. It is used to make
    -- sensible artist assignments later.
    -------------------------------------------------------------------------

    DECLARE @ArtistSeed TABLE
    (
        Name  varchar(50) NOT NULL PRIMARY KEY,
        Genre varchar(20) NOT NULL
    );

    INSERT INTO @ArtistSeed (Name, Genre)
    VALUES

    -- Jazz
    ('Nora Vale', 'Jazz'),
    ('Elias Hart', 'Jazz'),
    ('Theo Mercer', 'Jazz'),
    ('Lena Brooks', 'Jazz'),
    ('Martin Kovac', 'Jazz'),

    -- Comedy
    ('Priya Shah', 'Comedy'),
    ('Ben Carter', 'Comedy'),
    ('Lucy Martin', 'Comedy'),
    ('Tom Hargreaves', 'Comedy'),
    ('Aaron Blake', 'Comedy'),

    -- Buskers
    ('Milo Grant', 'Buskers'),
    ('Nina Rossi', 'Buskers'),
    ('Felix Novak', 'Buskers'),
    ('Hana Voss', 'Buskers'),
    ('Jules Turner', 'Buskers'),

    -- Rock
    ('Northern Static', 'Rock'),
    ('Velvet Voltage', 'Rock'),
    ('The Midnight Engines', 'Rock'),
    ('Red Lanterns', 'Rock'),
    ('Black River Club', 'Rock'),

    -- Family
    ('Pip and Pals', 'Family'),
    ('Sunny Sparks', 'Family'),
    ('The Paper Planes', 'Family'),
    ('Captain Cloud', 'Family'),
    ('Melody Mouse', 'Family'),

    -- Singer / songwriter / folk / acoustic / soul
    ('Clara Wells', 'Singer'),
    ('Jonah Reed', 'Singer'),
    ('Elise Moreno', 'Singer'),
    ('Daniel Price', 'Singer'),
    ('Mia Foster', 'Singer'),

    -- Classical
    ('Sofia Lind', 'Classical'),
    ('Adrian Bell', 'Classical'),
    ('Maya Chen', 'Classical'),
    ('The Linden Quartet', 'Classical'),
    ('Oliver Grant', 'Classical'),

    -- Burlesque
    ('Ruby Blaze', 'Burlesque'),
    ('Ivy St James', 'Burlesque'),
    ('Lola Maren', 'Burlesque'),
    ('Celeste Noir', 'Burlesque'),
    ('Vera Valentine', 'Burlesque'),

    -- Variety / magic / cabaret
    ('Owen Clarke', 'Variety'),
    ('Sophie Hart', 'Variety'),
    ('Max Bell', 'Variety'),
    ('The Lantern Duo', 'Variety'),
    ('Grace Rowan', 'Variety'),

    -- Indie
    ('Rowan Cole', 'Indie'),
    ('June Mercer', 'Indie'),
    ('Silver Avenue', 'Indie'),
    ('The Side Streets', 'Indie'),
    ('Amber Lane', 'Indie'),

    -- Hosts
    ('Jamie Quinn', 'Host'),
    ('Morgan Reed', 'Host'),
    ('Alex Morgan', 'Host'),
    ('Sam Taylor', 'Host'),
    ('Casey Flynn', 'Host');

    INSERT INTO Artist (Name)
    SELECT Name
    FROM @ArtistSeed;

    -------------------------------------------------------------------------
    -- GENERATE PERFORMANCES
    --
    -- The number of performances depends on the length of the show:
    --
    -- Shorter shows:       2-3 performances
    -- Medium shows:        3-4 performances
    -- Longer shows:        4-5 performances
    --
    -- Gaps between performances are deliberately retained.
    -------------------------------------------------------------------------

    DECLARE
        @ShowNo                  int,
        @ShowId                  int,
        @ShowName                varchar(50),
        @ShowGenre               varchar(20),
        @ShowStart               datetime,
        @ShowEnd                 datetime,
        @TotalMinutes            int,
        @PerformanceCount        int,
        @PerformanceNumber       int,
        @GapMinutes              int,
        @ActiveMinutes            int,
        @PerformanceMinutes      int,
        @NonIntermissionCount    int,
        @CurrentStart            datetime,
        @CurrentEnd              datetime,
        @Description             varchar(100),
        @UseIntermission         bit;

    DECLARE ShowCursor CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT
            ShowNo,
            ShowId,
            Name,
            StartTime,
            EndTime,
            Genre
        FROM @Shows
        ORDER BY ShowNo;

    OPEN ShowCursor;

    FETCH NEXT FROM ShowCursor
    INTO
        @ShowNo,
        @ShowId,
        @ShowName,
        @ShowStart,
        @ShowEnd,
        @ShowGenre;

    WHILE @@FETCH_STATUS = 0
    BEGIN

        SET @TotalMinutes = DATEDIFF(minute, @ShowStart, @ShowEnd);

        ---------------------------------------------------------------------
        -- Decide how many performances this show has.
        ---------------------------------------------------------------------

        IF @TotalMinutes < 150
        BEGIN
            SET @PerformanceCount =
                2 + ABS(CAST(CHECKSUM(@ShowNo, @ShowName) AS bigint)) % 2;
        END
        ELSE IF @TotalMinutes < 210
        BEGIN
            SET @PerformanceCount =
                3 + ABS(CAST(CHECKSUM(@ShowNo, @ShowName) AS bigint)) % 2;
        END
        ELSE
        BEGIN
            SET @PerformanceCount =
                4 + ABS(CAST(CHECKSUM(@ShowNo, @ShowName) AS bigint)) % 2;
        END;

        ---------------------------------------------------------------------
        -- Some larger shows have an explicit intermission.
        ---------------------------------------------------------------------

        IF @PerformanceCount >= 4
           AND
           (
               @ShowNo % 3 = 0
               OR @ShowNo % 7 = 0
           )
        BEGIN
            SET @UseIntermission = 1;
        END
        ELSE
        BEGIN
            SET @UseIntermission = 0;
        END;

        ---------------------------------------------------------------------
        -- Gaps are 12, 15 or 18 minutes.
        ---------------------------------------------------------------------

        SET @GapMinutes =
            12 + ((@ShowNo % 3) * 3);

        ---------------------------------------------------------------------
        -- Work out how much actual performance time is available.
        ---------------------------------------------------------------------

        IF @UseIntermission = 1
        BEGIN
            SET @NonIntermissionCount = @PerformanceCount - 1;

            SET @ActiveMinutes =
                @TotalMinutes
                - (@GapMinutes * (@PerformanceCount - 1))
                - 15;

            SET @PerformanceMinutes =
                @ActiveMinutes / @NonIntermissionCount;
        END
        ELSE
        BEGIN
            SET @NonIntermissionCount = @PerformanceCount;

            SET @ActiveMinutes =
                @TotalMinutes
                - (@GapMinutes * (@PerformanceCount - 1));

            SET @PerformanceMinutes =
                @ActiveMinutes / @PerformanceCount;
        END;

        SET @PerformanceNumber = 1;
        SET @CurrentStart = @ShowStart;

        WHILE @PerformanceNumber <= @PerformanceCount
        BEGIN

            -----------------------------------------------------------------
            -- PERFORMANCE DESCRIPTION
            -----------------------------------------------------------------

            IF @UseIntermission = 1
               AND @PerformanceNumber = 2
            BEGIN
                SET @Description = 'Intermission & Host';
            END
            ELSE IF @PerformanceNumber = 1
            BEGIN
                SET @Description =
                    CASE @ShowGenre
                        WHEN 'Jazz'      THEN 'Opening Jazz Set'
                        WHEN 'Comedy'    THEN 'Opening Comedian'
                        WHEN 'Buskers'  THEN 'Opening Street Set'
                        WHEN 'Rock'     THEN 'Opening Band'
                        WHEN 'Family'   THEN 'Story and Song'
                        WHEN 'Singer'   THEN 'Opening Acoustic Set'
                        WHEN 'Classical'THEN 'Opening Chamber Set'
                        WHEN 'Burlesque'THEN 'Opening Variety Act'
                        WHEN 'Variety'  THEN 'Opening Variety Act'
                        WHEN 'Indie'    THEN 'Opening Indie Set'
                        ELSE 'Opening Set'
                    END;
            END
            ELSE IF @PerformanceNumber = @PerformanceCount
            BEGIN
                SET @Description =
                    CASE @ShowGenre
                        WHEN 'Jazz'      THEN 'Closing Jazz Set'
                        WHEN 'Comedy'    THEN 'Headliner'
                        WHEN 'Buskers'  THEN 'Buskers Finale'
                        WHEN 'Rock'     THEN 'Rock Finale'
                        WHEN 'Family'   THEN 'Grand Finale'
                        WHEN 'Singer'   THEN 'Final Song Set'
                        WHEN 'Classical'THEN 'Closing Performance'
                        WHEN 'Burlesque'THEN 'Grand Finale'
                        WHEN 'Variety'  THEN 'Grand Finale'
                        WHEN 'Indie'    THEN 'Closing Indie Set'
                        ELSE 'Closing Performance'
                    END;
            END
            ELSE
            BEGIN
                SET @Description =
                    CASE @ShowGenre

                        WHEN 'Jazz'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Jazz Quartet Feature'
                                    WHEN 1 THEN 'Guest Improvisation'
                                    ELSE 'Blue Note Trio'
                                END

                        WHEN 'Comedy'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Stand-Up Set'
                                    WHEN 1 THEN 'Guest Comedian'
                                    ELSE 'Audience Q&A'
                                END

                        WHEN 'Buskers'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Acoustic Street Set'
                                    WHEN 1 THEN 'Loop Station Set'
                                    ELSE 'Guest Busker'
                                END

                        WHEN 'Rock'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Support Set'
                                    WHEN 1 THEN 'Guitar Feature'
                                    ELSE 'Full Band Set'
                                END

                        WHEN 'Family'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Family Music Show'
                                    WHEN 1 THEN 'Storytelling and Songs'
                                    ELSE 'Comedy for Families'
                                END

                        WHEN 'Singer'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Singer-Songwriter Set'
                                    WHEN 1 THEN 'Duet Feature'
                                    ELSE 'Acoustic Songbook'
                                END

                        WHEN 'Classical'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Piano Recital'
                                    WHEN 1 THEN 'String Ensemble'
                                    ELSE 'Chamber Ensemble'
                                END

                        WHEN 'Burlesque'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Burlesque Feature'
                                    WHEN 1 THEN 'Duet Number'
                                    ELSE 'Guest Dance Feature'
                                END

                        WHEN 'Variety'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Magic and Variety'
                                    WHEN 1 THEN 'Cabaret Feature'
                                    ELSE 'Comedy and Variety'
                                END

                        WHEN 'Indie'
                            THEN
                                CASE ((@PerformanceNumber - 2) % 3)
                                    WHEN 0 THEN 'Indie Band Set'
                                    WHEN 1 THEN 'New Music Showcase'
                                    ELSE 'Indie Guest Set'
                                END

                        ELSE 'Feature Performance'
                    END;
            END;

            -----------------------------------------------------------------
            -- INTERMISSION IS 15 MINUTES.
            -----------------------------------------------------------------

            IF @UseIntermission = 1
               AND @PerformanceNumber = 2
            BEGIN
                SET @CurrentEnd =
                    DATEADD(minute, 15, @CurrentStart);
            END
            ELSE IF @PerformanceNumber = @PerformanceCount
            BEGIN
                -- Let the final performance absorb any rounding remainder.
                SET @CurrentEnd = @ShowEnd;
            END
            ELSE
            BEGIN
                SET @CurrentEnd =
                    DATEADD(minute, @PerformanceMinutes, @CurrentStart);
            END;

            INSERT INTO Performance
            (
                Description,
                StartTime,
                EndTime,
                Show_id
            )
            VALUES
            (
                @Description,
                @CurrentStart,
                @CurrentEnd,
                @ShowId
            );

            -----------------------------------------------------------------
            -- Leave a deliberate gap before the next performance.
            -----------------------------------------------------------------

            SET @CurrentStart =
                DATEADD(minute, @GapMinutes, @CurrentEnd);

            SET @PerformanceNumber = @PerformanceNumber + 1;

        END;

        FETCH NEXT FROM ShowCursor
        INTO
            @ShowNo,
            @ShowId,
            @ShowName,
            @ShowStart,
            @ShowEnd,
            @ShowGenre;
    END;

    CLOSE ShowCursor;
    DEALLOCATE ShowCursor;

    -------------------------------------------------------------------------
    -- ASSIGN ARTISTS TO PERFORMANCES
    --
    -- Normal performances receive 3 artists.
    -- Intermissions receive exactly 1 Host.
    --
    -- @Shows contains the Genre information because Genre is not a
    -- column in the actual Show table.
    -------------------------------------------------------------------------

    ;WITH PerformanceTargets AS
    (
        SELECT
            p.Id AS PerformanceId,

            CASE
                WHEN p.Description = 'Intermission & Host'
                    THEN 'Host'
                ELSE seedShow.Genre
            END AS Genre,

            CASE
                WHEN p.Description = 'Intermission & Host'
                    THEN 1
                ELSE 3
            END AS ArtistCount

        FROM Performance p

        INNER JOIN Show s
            ON s.id = p.Show_id

        INNER JOIN @Shows seedShow
            ON seedShow.ShowId = s.id
    ),

    ArtistsByGenre AS
    (
        SELECT
            a.id AS ArtistId,
            seed.Genre,

            ROW_NUMBER() OVER
            (
                PARTITION BY seed.Genre
                ORDER BY a.id
            ) AS ArtistNumber

        FROM Artist a

        INNER JOIN @ArtistSeed seed
            ON seed.Name = a.Name
    ),

    RankedAssignments AS
    (
        SELECT
            pt.PerformanceId,
            abg.ArtistId,
            pt.ArtistCount,

            ROW_NUMBER() OVER
            (
                PARTITION BY pt.PerformanceId
                ORDER BY abg.ArtistNumber
            ) AS AssignmentNumber

        FROM PerformanceTargets pt

        INNER JOIN ArtistsByGenre abg
            ON abg.Genre = pt.Genre
    )

    INSERT INTO ArtistPerformance
    (
        RoyaltyUpFront,
        RoyaltyAtEnd,
        Artist_id,
        Performance_id
    )
    SELECT
        40 + ((ra.ArtistId * 13 + ra.PerformanceId * 7) % 90),
        20 + ((ra.ArtistId * 5 + ra.PerformanceId * 11) % 120),
        ra.ArtistId,
        ra.PerformanceId

    FROM RankedAssignments ra

    WHERE ra.AssignmentNumber <= ra.ArtistCount;


    -------------------------------------------------------------------------
    -- VALIDATE ARTIST PERFORMANCE COUNT
    -------------------------------------------------------------------------

    SELECT COUNT(*) AS ArtistPerformanceCount
    FROM ArtistPerformance;




    -------------------------------------------------------------------------
    -- SEATS
    --
    -- 6 rows x 10 seats = 60 seats.
    -------------------------------------------------------------------------

    INSERT INTO Seat
    (
        [Row],
        [Number]
    )
    SELECT
        r.RowName,
        n.SeatNumber
    FROM
    (
        VALUES
            ('A'),
            ('B'),
            ('C'),
            ('D'),
            ('E'),
            ('F')
    ) AS r(RowName)
    CROSS JOIN
    (
        VALUES
            (1),
            (2),
            (3),
            (4),
            (5),
            (6),
            (7),
            (8),
            (9),
            (10)
    ) AS n(SeatNumber);


    -------------------------------------------------------------------------
    -- TICKETHOLDERS
    -------------------------------------------------------------------------

    INSERT INTO Ticketholder
    (
        Name,
        Birthdate,
        Email,
        Discount
    )
    VALUES

    ('Anna Berger',
        '1987-03-14',
        'anna.berger@example.com',
        0),

    ('Martin Hofer',
        '1979-11-22',
        'martin.hofer@example.com',
        0),

    ('Laura Weiss',
        '1992-07-08',
        'laura.weiss@example.com',
        1),

    ('Daniel Gruber',
        '1984-01-19',
        'daniel.gruber@example.com',
        0),

    ('Sophie Wagner',
        '1995-05-27',
        'sophie.wagner@example.com',
        0),

    ('Thomas Leitner',
        '1973-09-03',
        'thomas.leitner@example.com',
        0),

    ('Julia Moser',
        '1990-12-11',
        'julia.moser@example.com',
        1),

    ('David Steiner',
        '1988-06-24',
        'david.steiner@example.com',
        0),

    ('Maria Hofer',
        '1968-02-16',
        'maria.hofer@example.com',
        1),

    ('Felix Bauer',
        '1997-08-30',
        'felix.bauer@example.com',
        0),

    ('Clara Schmidt',
        '1985-04-12',
        'clara.schmidt@example.com',
        0),

    ('Markus Fuchs',
        '1976-10-05',
        'markus.fuchs@example.com',
        0),

    ('Eva Steiner',
        '1993-01-29',
        'eva.steiner@example.com',
        1),

    ('Lukas Pichler',
        '1991-06-17',
        'lukas.pichler@example.com',
        0),

    ('Nina Bauer',
        '1989-09-21',
        'nina.bauer@example.com',
        0),

    ('Johannes Koch',
        '1982-12-02',
        'johannes.koch@example.com',
        0),

    ('Elena Novak',
        '1996-03-07',
        'elena.novak@example.com',
        1),

    ('Peter Huber',
        '1971-05-18',
        'peter.huber@example.com',
        0),

    ('Sarah Maier',
        '1994-11-13',
        'sarah.maier@example.com',
        0),

    ('Andreas Bauer',
        '1980-07-26',
        'andreas.bauer@example.com',
        0),

    ('Katharina Lang',
        '1986-02-04',
        'katharina.lang@example.com',
        1),

    ('Michael Wimmer',
        '1978-08-15',
        'michael.wimmer@example.com',
        0),

    ('Lisa Schmid',
        '1998-10-09',
        'lisa.schmid@example.com',
        0),

    ('Stefan Auer',
        '1983-04-28',
        'stefan.auer@example.com',
        0),

    ('Emma Richter',
        '1999-12-20',
        'emma.richter@example.com',
        1);


    -------------------------------------------------------------------------
    -- TICKETS
    --
    -- Each show gets between 6 and 14 tickets.
    --
    -- The formulas deliberately reuse ticketholders across many shows,
    -- producing realistic repeat customers.
    --
    -- Seat numbers are unique within each show.
    -------------------------------------------------------------------------

    ;WITH TicketNumbers AS
    (
        SELECT n
        FROM
        (
            VALUES
                (1),
                (2),
                (3),
                (4),
                (5),
                (6),
                (7),
                (8),
                (9),
                (10),
                (11),
                (12),
                (13),
                (14)
        ) AS v(n)
    ),

    ShowTicketCounts AS
    (
        SELECT
            s.id AS ShowId,
            6 + ((s.id * 7) % 9) AS TicketCount
        FROM Show s
    )

    INSERT INTO Ticket
    (
        Ticketholder_id,
        Seat_id,
        Show_id
    )
    SELECT
        th.id AS TicketholderId,
        seat.id AS SeatId,
        stc.ShowId

    FROM ShowTicketCounts stc

    INNER JOIN TicketNumbers tn
        ON tn.n <= stc.TicketCount

    INNER JOIN Ticketholder th
        ON th.id =
            1 +
            (
                (stc.ShowId * 3 + tn.n * 5)
                % 25
            )

    INNER JOIN Seat seat
        ON seat.id =
            1 +
            (
                (stc.ShowId * 17 + tn.n * 7)
                % 60
            );


    -------------------------------------------------------------------------
    -- VALIDATION
    -------------------------------------------------------------------------

    PRINT '========================================';
    PRINT 'SINGLE STAGE TEST DATA CREATED';
    PRINT '========================================';


    -------------------------------------------------------------------------
    -- BASIC COUNTS
    -------------------------------------------------------------------------

    SELECT
        'Shows' AS Entity,
        COUNT(*) AS [Count]
    FROM Show

    UNION ALL

    SELECT
        'Performances',
        COUNT(*)
    FROM Performance

    UNION ALL

    SELECT
        'Artists',
        COUNT(*)
    FROM Artist

    UNION ALL

    SELECT
        'ArtistPerformances',
        COUNT(*)
    FROM ArtistPerformance

    UNION ALL

    SELECT
        'Seats',
        COUNT(*)
    FROM Seat

    UNION ALL

    SELECT
        'Ticketholders',
        COUNT(*)
    FROM Ticketholder

    UNION ALL

    SELECT
        'Tickets',
        COUNT(*)
    FROM Ticket;


    -------------------------------------------------------------------------
    -- SHOW SCHEDULE
    -------------------------------------------------------------------------

    SELECT
        s.id,
        s.Name,
        s.StartTime,
        s.EndTime,
        s.TicketPrice,
        s.SoldOut,
        COUNT(p.id) AS PerformanceCount
    FROM Show s
    LEFT JOIN Performance p
        ON p.Show_id = s.id
    GROUP BY
        s.id,
        s.Name,
        s.StartTime,
        s.EndTime,
        s.TicketPrice,
        s.SoldOut
    ORDER BY
        s.StartTime;


    -------------------------------------------------------------------------
    -- CHECK THAT EVERY SHOW HAS 2-5 PERFORMANCES
    -------------------------------------------------------------------------

    SELECT
        s.Name,
        COUNT(p.id) AS PerformanceCount
    FROM Show s
    LEFT JOIN Performance p
        ON p.Show_id = s.id
    GROUP BY
        s.Name
    HAVING COUNT(p.id) < 2
        OR COUNT(p.id) > 5;


    -------------------------------------------------------------------------
    -- CHECK THAT PERFORMANCES STAY INSIDE THEIR SHOW
    --
    -- Expected result: zero rows.
    -------------------------------------------------------------------------

    SELECT
        s.Name AS ShowName,
        p.Description,
        p.StartTime,
        p.EndTime
    FROM Performance p
    INNER JOIN Show s
        ON s.id = p.Show_id
    WHERE
        p.StartTime < s.StartTime
        OR p.EndTime > s.EndTime;


    -------------------------------------------------------------------------
    -- CHECK FOR OVERLAPPING PERFORMANCES
    --
    -- Expected result: zero rows.
    -------------------------------------------------------------------------

    SELECT
        s.Name AS ShowName,
        p1.Description AS Performance1,
        p1.StartTime AS Start1,
        p1.EndTime AS End1,
        p2.Description AS Performance2,
        p2.StartTime AS Start2,
        p2.EndTime AS End2
    FROM Performance p1
    INNER JOIN Performance p2
        ON p1.Show_id = p2.Show_id
        AND p1.id < p2.id
        AND p1.StartTime < p2.EndTime
        AND p1.EndTime > p2.StartTime
    INNER JOIN Show s
        ON s.id = p1.Show_id;


    -------------------------------------------------------------------------
    -- CHECK FOR OVERLAPPING SHOWS
    --
    -- Expected result: zero rows.
    -------------------------------------------------------------------------

    SELECT
        s1.Name AS Show1,
        s1.StartTime AS Show1Start,
        s1.EndTime AS Show1End,
        s2.Name AS Show2,
        s2.StartTime AS Show2Start,
        s2.EndTime AS Show2End
    FROM Show s1
    INNER JOIN Show s2
        ON s1.id < s2.id
        AND s1.StartTime < s2.EndTime
        AND s1.EndTime > s2.StartTime;


    -------------------------------------------------------------------------
    -- CHECK ARTIST COUNTS
    --
    -- Expected: every performance between 1 and 3 artists.
    -------------------------------------------------------------------------

    SELECT
        p.id AS PerformanceId,
        p.Description,
        COUNT(ap.id) AS ArtistCount
    FROM Performance p
    LEFT JOIN ArtistPerformance ap
        ON ap.Performance_id = p.id
    GROUP BY
        p.id,
        p.Description
    HAVING COUNT(ap.id) < 1
        OR COUNT(ap.id) > 3;


    -------------------------------------------------------------------------
    -- TICKETHOLDER PURCHASE HISTORY
    --
    -- Useful for checking that repeat customers exist.
    -------------------------------------------------------------------------

    SELECT
        th.id,
        th.Name,
        th.Email,
        COUNT(t.id) AS TicketCount
    FROM Ticketholder th
    LEFT JOIN Ticket t
        ON t.Ticketholder_id = th.id
    GROUP BY
        th.id,
        th.Name,
        th.Email
    ORDER BY
        TicketCount DESC,
        th.Name;


    -------------------------------------------------------------------------
    -- SOLD OUT SHOWS
    --
    -- These are intentionally independent of ticket sales.
    -------------------------------------------------------------------------

    SELECT
        id,
        Name,
        StartTime,
        TicketPrice,
        SoldOut,
        (
            SELECT COUNT(*)
            FROM Ticket t
            WHERE t.Show_id = Show.id
        ) AS ActualTicketCount
    FROM Show
    WHERE SoldOut = 1
    ORDER BY StartTime;


    -------------------------------------------------------------------------
    -- COMMIT
    -------------------------------------------------------------------------

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    IF CURSOR_STATUS('local', 'ShowCursor') >= -1
    BEGIN
        IF CURSOR_STATUS('local', 'ShowCursor') > -1
            CLOSE ShowCursor;

        DEALLOCATE ShowCursor;
    END;

    THROW;

END CATCH;
GO
