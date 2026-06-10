export const languages = [
  { code: "vi", label: "Tieng Viet" },
  { code: "en", label: "English" },
];

export const mockPois = [
  {
    id: 1,
    latitude: 10.762622,
    longitude: 106.660172,
    radius: 60,
    status: "active",
    qr_code_token: "POI-CH-001",
    translations: {
      vi: {
        name: "Dinh Doc Lap",
        short_description: "Diem tham quan lich su trung tam thanh pho.",
        full_description: "Cong trinh gan voi nhieu dau moc lich su quan trong cua Sai Gon.",
        audio_url: "/audio/dinh-doc-lap-vi.mp3",
      },
      en: {
        name: "Independence Palace",
        short_description: "Historic landmark in the city center.",
        full_description: "A signature site connected with key milestones in Saigon history.",
        audio_url: "/audio/independence-palace-en.mp3",
      },
    },
  },
  {
    id: 2,
    latitude: 10.779784,
    longitude: 106.699018,
    radius: 45,
    status: "active",
    qr_code_token: "POI-ND-002",
    translations: {
      vi: {
        name: "Nha tho Duc Ba",
        short_description: "Bieu tuong kien truc Phap tai trung tam TP.HCM.",
        full_description: "Khong gian ton giao va kien truc noi bat tren truc duong Cong xa Paris.",
        audio_url: "/audio/nha-tho-duc-ba-vi.mp3",
      },
      en: {
        name: "Notre-Dame Cathedral Basilica",
        short_description: "French colonial architectural icon in Ho Chi Minh City.",
        full_description: "A prominent religious and architectural site on Paris Commune Square.",
        audio_url: "/audio/notre-dame-en.mp3",
      },
    },
  },
  {
    id: 3,
    latitude: 10.775658,
    longitude: 106.700424,
    radius: 55,
    status: "hidden",
    qr_code_token: "POI-BD-003",
    translations: {
      vi: {
        name: "Buu dien Thanh pho",
        short_description: "Cong trinh co dien gan Nha tho Duc Ba.",
        full_description: "Diem dung chan quen thuoc voi mat dung vang va khong gian sanh lon.",
        audio_url: "/audio/buu-dien-vi.mp3",
      },
      en: {
        name: "Central Post Office",
        short_description: "Classic heritage building near Notre-Dame Cathedral.",
        full_description: "A familiar stop with a bright facade and grand public hall.",
        audio_url: "/audio/post-office-en.mp3",
      },
    },
  },
];

export const mockTours = [
  {
    id: 1,
    estimated_time: 90,
    status: "active",
    translations: {
      vi: {
        title: "Dau an Sai Gon xua",
        description: "Lo trinh tham quan cac cong trinh bieu tuong cua trung tam thanh pho.",
      },
      en: {
        title: "Old Saigon Highlights",
        description: "A route through iconic heritage landmarks in the city center.",
      },
    },
    poiIds: [1, 2, 3],
  },
  {
    id: 2,
    estimated_time: 45,
    status: "active",
    translations: {
      vi: {
        title: "Kien truc thuoc dia",
        description: "Tour ngan danh cho khach muon tap trung vao cac diem kien truc Phap.",
      },
      en: {
        title: "Colonial Architecture Walk",
        description: "A compact tour focused on French colonial architecture.",
      },
    },
    poiIds: [2, 3],
  },
];

export const playbackSeries = [
  { day: "Mon", listens: 124, avgTime: 188 },
  { day: "Tue", listens: 156, avgTime: 201 },
  { day: "Wed", listens: 142, avgTime: 194 },
  { day: "Thu", listens: 212, avgTime: 224 },
  { day: "Fri", listens: 238, avgTime: 231 },
  { day: "Sat", listens: 316, avgTime: 246 },
  { day: "Sun", listens: 281, avgTime: 239 },
];

export const heatmapPoints = [
  { id: 1, visitor_latitude: 10.7628, visitor_longitude: 106.6604, listen_duration: 210 },
  { id: 2, visitor_latitude: 10.7795, visitor_longitude: 106.6992, listen_duration: 185 },
  { id: 3, visitor_latitude: 10.7758, visitor_longitude: 106.7007, listen_duration: 240 },
  { id: 4, visitor_latitude: 10.7761, visitor_longitude: 106.6999, listen_duration: 166 },
];
