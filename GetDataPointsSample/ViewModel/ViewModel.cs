using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GetDataPointsSample
{
    public class ScatterSeriesViewModel 
    {
        public ObservableCollection<ChartDataModel> Data { get; set; }

        public ScatterSeriesViewModel()
        {
            Data = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel( 161, 65 ), new ChartDataModel( 150,  65 ), new ChartDataModel(155,  65 ), new ChartDataModel(160, 65 ),
                new ChartDataModel( 148, 66 ), new ChartDataModel( 145,  66 ), new ChartDataModel(137,  66 ), new ChartDataModel(138, 66 ),
                new ChartDataModel( 162, 66 ), new ChartDataModel( 166,  66 ), new ChartDataModel(159,  66 ), new ChartDataModel(151, 66 ),
                new ChartDataModel( 180, 66 ), new ChartDataModel( 181,  66 ), new ChartDataModel(174,  66 ), new ChartDataModel(159, 66 ),
                new ChartDataModel( 151, 67 ), new ChartDataModel( 148,  67 ), new ChartDataModel(141,  67 ), new ChartDataModel(145, 67 ),
                new ChartDataModel( 165, 67 ), new ChartDataModel( 168,  67 ), new ChartDataModel(159,  67 ), new ChartDataModel(183, 67 ),
                new ChartDataModel( 188, 67 ), new ChartDataModel( 187,  67 ), new ChartDataModel(172,  67 ), new ChartDataModel(193, 67 ),
                new ChartDataModel( 153, 68 ), new ChartDataModel( 153,  68 ), new ChartDataModel(147,  68 ), new ChartDataModel(163, 68 ),
                new ChartDataModel( 174, 68 ), new ChartDataModel( 173,  68 ), new ChartDataModel(160,  68 ), new ChartDataModel(191, 68 ),
                new ChartDataModel( 131, 62 ), new ChartDataModel( 140,  62 ), new ChartDataModel(149,  62 ), new ChartDataModel(115, 62 ),
                new ChartDataModel( 164, 63 ), new ChartDataModel( 162,  63 ), new ChartDataModel(167,  63 ), new ChartDataModel(146, 63 ),
                new ChartDataModel( 150, 64 ), new ChartDataModel( 141,  64 ), new ChartDataModel(142,  64 ), new ChartDataModel(129, 64 ),
                new ChartDataModel( 159, 64 ), new ChartDataModel( 158,  64 ), new ChartDataModel(162,  64 ), new ChartDataModel(136, 64 ),
                new ChartDataModel( 176, 64 ), new ChartDataModel( 170,  64 ), new ChartDataModel(167,  64 ), new ChartDataModel(144, 64 ),
                new ChartDataModel( 143, 65 ), new ChartDataModel( 137,  65 ), new ChartDataModel(137,  65 ), new ChartDataModel(140, 65 ),
                new ChartDataModel( 182, 65 ), new ChartDataModel( 168,  65 ), new ChartDataModel(181,  65 ), new ChartDataModel(165, 65 ),
                new ChartDataModel( 214, 74 ), new ChartDataModel( 211,  74 ), new ChartDataModel(166,  74 ), new ChartDataModel(185, 74 ),
                new ChartDataModel( 189, 68 ), new ChartDataModel( 182,  68 ), new ChartDataModel(181,  68 ), new ChartDataModel(196, 68 ),
                new ChartDataModel( 152, 69 ), new ChartDataModel( 173,  69 ), new ChartDataModel(190,  69 ), new ChartDataModel(161, 69 ),
                new ChartDataModel( 173, 69 ), new ChartDataModel( 185,  69 ), new ChartDataModel(141,  69 ), new ChartDataModel(149, 69 ),
                new ChartDataModel( 134, 62 ), new ChartDataModel( 183,  62 ), new ChartDataModel(155,  62 ), new ChartDataModel(164, 62 ),
                new ChartDataModel( 169, 62 ), new ChartDataModel( 122,  62 ), new ChartDataModel(161,  62 ), new ChartDataModel(166, 62 ),
                new ChartDataModel( 137, 63 ), new ChartDataModel( 140,  63 ), new ChartDataModel(140,  63 ), new ChartDataModel(126, 63 ),
                new ChartDataModel( 150, 63 ), new ChartDataModel( 153,  63 ), new ChartDataModel(154,  63 ), new ChartDataModel(139, 63 ),
                new ChartDataModel( 186, 69 ), new ChartDataModel( 188,  69 ), new ChartDataModel(148,  69 ), new ChartDataModel(174, 69 ),
                new ChartDataModel( 164, 70 ), new ChartDataModel( 182,  70 ), new ChartDataModel(200,  70 ), new ChartDataModel(151, 70 ),
                new ChartDataModel( 204, 74 ), new ChartDataModel( 177,  74 ), new ChartDataModel(194,  74 ), new ChartDataModel(212, 74 ),
                new ChartDataModel( 162, 70 ), new ChartDataModel( 200,  70 ), new ChartDataModel(166,  70 ), new ChartDataModel(177, 70 ),
                new ChartDataModel( 188, 70 ), new ChartDataModel( 156,  70 ), new ChartDataModel(175,  70 ), new ChartDataModel(191, 70 ),
                new ChartDataModel( 174, 71 ), new ChartDataModel( 187,  71 ), new ChartDataModel(208,  71 ), new ChartDataModel(166, 71 ),
                new ChartDataModel( 150, 71 ), new ChartDataModel( 194,  71 ), new ChartDataModel(157,  71 ), new ChartDataModel(183, 71 ),
                new ChartDataModel( 204, 71 ), new ChartDataModel( 162,  71 ), new ChartDataModel(179,  71 ), new ChartDataModel(196, 71 ),
                new ChartDataModel( 170, 72 ), new ChartDataModel( 184,  72 ), new ChartDataModel(197,  72 ), new ChartDataModel(162, 72 ),
                new ChartDataModel( 177, 72 ), new ChartDataModel( 203,  72 ), new ChartDataModel(159,  72 ), new ChartDataModel(178, 72 ),
                new ChartDataModel( 198, 72 ), new ChartDataModel( 167,  72 ), new ChartDataModel(184,  72 ), new ChartDataModel(201, 72 ),
                new ChartDataModel( 167, 73 ), new ChartDataModel( 178,  73 ), new ChartDataModel(215,  73 ), new ChartDataModel(207, 73 ),
                new ChartDataModel( 172, 73 ), new ChartDataModel( 204,  73 ), new ChartDataModel(162,  73 ), new ChartDataModel(182, 73 ),
                new ChartDataModel( 201, 73 ), new ChartDataModel( 172,  73 ), new ChartDataModel(189,  73 ), new ChartDataModel(206, 73 ),
                new ChartDataModel( 150, 74 ), new ChartDataModel( 187,  74 ), new ChartDataModel(153,  74 ), new ChartDataModel(171, 74 ),
            };
        }
    }

}
