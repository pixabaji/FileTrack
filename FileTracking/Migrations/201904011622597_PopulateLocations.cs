namespace FileTracking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateLocations : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COBUE', 1, 'Buena Vista (CZL)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCAL', 1, 'Calcutta')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCAR', 1, 'Carolina')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCHA', 1, 'Chan Chen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCHU', 1, 'Chunox')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCLD', 1, 'Caledonia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCOB', 1, 'Copper Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCOJ', 1, 'Consejo')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCOP', 1, 'Concepcion')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCOR', 1, 'Corozal Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COCRI', 1, 'Cristo Rey (CZL)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COLIB', 1, 'Libertad')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COLIT', 1, 'Little Belize')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COLOU', 1, 'Louisville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COPAR', 1, 'Para�so')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COPAT', 1, 'Patchakan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COPED', 1, 'San Pedro')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COPRO', 1, 'Progresso')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CORAN', 1, 'Ranchito')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COROM', 1, 'San Roman (CZL)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSAN', 1, 'San Andres')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSAT', 1, 'San Antonio (CZL)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSCL', 1, 'Santa Clara')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSJO', 1, 'San Joaquin')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSNA', 1, 'San Narciso')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COSTE', 1, 'Sarteneja')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COVIC', 1, 'San V�ctor')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COXAI', 1, 'Xaibe')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('COYO', 1, 'Yo Chen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CZELE', 1, 'Santa Elena (CZL)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UACZ', 1, 'Unnamed Area - Corozal District')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORAUG', 2, 'August Pine Ridge')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORBLU', 2, 'Blue Creek (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORCAR', 2, 'Carmelita')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORCHA', 2, 'Chan Pine Ridge')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORDOU', 2, 'Douglas')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORFIR', 2, 'Fire Burn')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORGUI', 2, 'Guinea Grass')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORINC', 2, 'Indian Church')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORIND', 2, 'Indian Creek (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORNEU', 2, 'Neuland Community')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORNUE', 2, 'Nuevo San Juan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORORA', 2, 'Orange Walk Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORPAL', 2, 'Palmar')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSAN', 2, 'San Antonio (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSCR', 2, 'San Carlos')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSCZ', 2, 'Santa Cruz (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSES', 2, 'San Estevan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSFE', 2, 'San Felipe (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSHI', 2, 'Shipyard')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSJO', 2, 'San Jose (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSLU', 2, 'San Luis')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSLZ', 2, 'San Lazaro')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSMR', 2, 'Santa Martha')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSPL', 2, 'San Pablo (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORSRO', 2, 'San Roman (OWK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORTOW', 2, 'Tower Hill')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORTRF', 2, 'Trial Farm')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORTRI', 2, 'Trinidad')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ORYO', 2, 'Yo Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UAOW', 2, 'Unnamed Area - Orange Walk')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('ACC', 3, 'Ambergris Caye')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEANN', 3, 'St Anns')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBEL', 3, 'Belize City')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBER', 3, 'Bermudian Landing')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBIS', 3, 'Biscayne')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBOM', 3, 'Bomba')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBOS', 3, 'Boston')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEBUR', 3, 'Burrell Boom')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BECAY', 3, 'Caye Caulker')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BECOR', 3, 'Corozalito')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BECRO', 3, 'Crooked Tree')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEDOU', 3, 'Double Head Cabbage')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEFLO', 3, 'Flowers Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEFRE', 3, 'Freetown Sibun')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEGAL', 3, 'Gales Point')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEGAR', 3, 'Gardenia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEGEO', 3, 'St Georges Caye')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEGRA', 3, 'Gracie Rock')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEHAT', 3, 'Hattieville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEISA', 3, 'Isabella Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BELAD', 3, 'La Democracia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BELAV', 3, 'Ladyville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BELEM', 3, 'Lemonal')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BELOR', 3, 'Lords Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BELUC', 3, 'Lucky Strike')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEMAH', 3, 'Mahogany Heights')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEMAS', 3, 'Maskall')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEMAY', 3, 'May Pen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEPAU', 3, 'St Pauls Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BERAN', 3, 'Rancho Dolores')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEROC', 3, 'Rockstone Pond')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BESAN', 3, 'Sandhill')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BESCO', 3, 'Scotland Half Moon')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BESNT', 3, 'Santana')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BESPE', 3, 'San Pedro Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEWES', 3, 'Western Paradise')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('BEWIL', 3, 'Willows Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UABZ', 3, 'Unnamed Area - Belize District')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAANT', 4, 'San Antonio (CYO)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAARE', 4, 'Arenal')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAARM', 4, 'Armenia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABEL', 4, 'Belmopan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABEN', 4, 'Benque Viejo del Carmen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABIL', 4, 'Billy White')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABLA', 4, 'Blackman Eddy')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABUE', 4, 'Buena Vista (CYO)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CABUL', 4, 'Bullet Tree')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CACAL', 4, 'Calla Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CACAM', 4, 'Camalote')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CACOT', 4, 'Cotton Tree')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CACRI', 4, 'Cristo Rey (CYO)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CADC1', 4, 'Duck Run I')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CADC2', 4, 'Duck Run II')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CADC3', 4, 'Duck Run III')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAELE', 4, 'Santa Elena Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAELP', 4, 'El Progresso')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAESP', 4, 'Esperanza')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAFAM', 4, 'Santa Familia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAFRA', 4, 'Franks Eddy')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAGEO', 4, 'Georgeville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAIGN', 4, 'San Ignacio Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CALAG', 4, 'La Gracia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CALOS', 4, 'Los Tambos')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAMAR', 4, 'St Margaret')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAMAS', 4, 'San Marcos (CYO)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAMAT', 4, 'St Mathews')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAMOR', 4, 'More Tomorrow')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAONT', 4, 'Ontario')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAROA', 4, 'Roaring Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CASEL', 4, 'Selena')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CASPA', 4, 'Spanish Lookout')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CASPR', 4, 'Springfield')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CASUC', 4, 'Succotz')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CATEA', 4, 'Teakettle')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CATER', 4, 'Santa Teresita')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAUNI', 4, 'Unitedville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAVAL', 4, 'Valley of Peace')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CAYAL', 4, 'Yalbac')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CYCF', 4, 'Central Farm')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('CYSM', 4, 'Seven Miles Village')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UACY', 4, 'Unnamed Area - Cayo District')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('SCCOW', 5, 'Cowpen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('SCR', 5, 'Stann Creek Rural')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STALT', 5, 'Alta Vista')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STDAN', 5, 'Dangriga Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STGEO', 5, 'George Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STHOP', 5, 'Hope Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STHPK', 5, 'Hopkins')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STHUM', 5, 'Hummingbird')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STIND', 5, 'Independence')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STMAR', 5, 'Maya Center')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STMAY', 5, 'Maya Mopan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STMID', 5, 'Middlesex')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STNEW', 5, 'New Mullins River')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STPLA', 5, 'Placencia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STPOM', 5, 'Pomona')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STRED', 5, 'Red Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSAR', 5, 'Sarawee')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSCR', 5, 'Santa Cruz (SCK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSEI', 5, 'Seine Bight')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSIL', 5, 'Silk Grass')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSIT', 5, 'Sittee River')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSJN', 5, 'San Juan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSRA', 5, 'Santa Rosa')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSRO', 5, 'San Roman (SCK)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STSTE', 5, 'Steadfast')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('STVAL', 5, 'Valley Community')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UASC', 5, 'Unnamed Area - Stann Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOACR', 6, 'Santa Cruz (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOAEL', 6, 'Santa Elena (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOAGU', 6, 'Aguacate')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOANA', 6, 'Santa Ana')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOANT', 6, 'San Antonio (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOATE', 6, 'Santa Teresa')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBAR', 6, 'Barranco')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBEL', 6, 'Bella Vista')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBEN', 6, 'San Benito Poite')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBIG', 6, 'Big Falls')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBLA', 6, 'Bladen')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBLU', 6, 'Blue Creek (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOBOO', 6, 'Boom Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOCAT', 6, 'Cattle Landing')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOCON', 6, 'Conejo Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOCOR', 6, 'Corazon Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOCRJ', 6, 'Crique Jute')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOCRS', 6, 'Crique Sarco')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TODOL', 6, 'Dolores')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOELD', 6, 'Elridgeville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOFEL', 6, 'San Felipe (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOFOR', 6, 'Forest Home')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOGOL', 6, 'Golden Stream')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOIND', 6, 'Indian Creek (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOJAC', 6, 'Jacintoville')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOJAL', 6, 'Jalacte')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOJOR', 6, 'Jordan')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOJOS', 6, 'San Jose (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOLAG', 6, 'Laguna')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOLUC', 6, 'San Lucas')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMAB', 6, 'Mabil Ha')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMAF', 6, 'Mafredi')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMAR', 6, 'San Marcos (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMED', 6, 'Medina Bank')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMID', 6, 'Midway')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMIG', 6, 'San Miguel')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOMON', 6, 'Monkey River')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TONA', 6, 'Na Luum Ca')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOOTO', 6, 'Otoxha')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOPAB', 6, 'San Pablo (TLD)')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOPED', 6, 'San Pedro Columbia')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOPUE', 6, 'Pueblo Viejo')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOPUN', 6, 'Punta Negra')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOPUT', 6, 'Punta Gorda Town')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOSIL', 6, 'Silver Creek')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOSNI', 6, 'San Isidro')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOSUN', 6, 'Sunday Wood')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOTRI', 6, 'Trio')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOVIC', 6, 'San Vicente')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('TOYEM', 6, 'Yemeri Grove')");
            Sql("INSERT INTO Locations(LocationId, DistrictsId, Name) VALUES ('UATL', 6, 'Unnamed Area - Toledo District')");

        }

        public override void Down()
        {
        }
    }
}
