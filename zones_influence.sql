CREATE TABLE `zones_influence` (
  `id` int(11) NOT NULL,
  `gang` varchar(80) DEFAULT 'none',
  `val` float DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

INSERT INTO `zones_influence` (`id`, `gang`, `val`) VALUES
(4, 'none', 0),
(11, 'none', 0),
(18, 'none', 0),
(25, 'none', 0),
(32, 'none', 0),
(39, 'none', 0),
(46, 'none', 0),
(53, 'none', 0),
(60, 'none', 0),
(67, 'none', 0),
(74, 'none', 0),
(81, 'none', 0),
(88, 'none', 0),
(95, 'none', 0),
(102, 'none', 0),
(109, 'none', 0),
(116, 'none', 0),
(123, 'none', 0),
(130, 'none', 0),
(137, 'none', 0),
(144, 'none', 0),
(151, 'none', 0),
(158, 'none', 0),
(165, 'none', 0),
(172, 'none', 0),
(179, 'none', 0),
(186, 'none', 0),
(193, 'none', 0),
(200, 'none', 0),
(207, 'none', 0),
(214, 'none', 0),
(221, 'none', 0),
(228, 'none', 0),
(235, 'none', 0),
(1, 'none', 0),
(2, 'none', 0),
(249, 'none', 0);

ALTER TABLE `zones_influence`
  ADD PRIMARY KEY (`id`);
COMMIT;
