CREATE TABLE `zones_influence` (
  `id` int(11) NOT NULL,
  `gang` varchar(80) DEFAULT 'none',
  `val` float DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

INSERT INTO `zones_influence` (`id`, `gang`, `val`) VALUES
(21, 'none', 0),
(63, 'none', 0),
(77, 'none', 0),
(79, 'none', 0),
(95, 'none', 0),
(106, 'none', 0),
(111, 'none', 0),
(115, 'none', 0),
(119, 'none', 0),
(123, 'none', 0),
(129, 'none', 0),
(134, 'none', 0),
(137, 'none', 0),
(138, 'none', 0),
(144, 'none', 0),
(146, 'none', 0),
(147, 'none', 0),
(150, 'none', 0),
(153, 'none', 0),
(158, 'none', 0),
(160, 'none', 0),
(162, 'none', 0),
(166, 'none', 0),
(171, 'none', 0),
(178, 'none', 0),
(181, 'none', 0),
(182, 'none', 0),
(188, 'none', 0),
(198, 'none', 0),
(202, 'none', 0),
(204, 'none', 0),
(207, 'none', 0),
(208, 'none', 0),
(212, 'none', 0),
(1, 'none', 0),
(2, 'none', 0),
(216, 'none', 0);

ALTER TABLE `zones_influence`
  ADD PRIMARY KEY (`id`);
COMMIT;
