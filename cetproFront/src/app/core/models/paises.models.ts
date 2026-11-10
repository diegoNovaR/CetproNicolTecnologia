export interface IdiomaPais {
  name: string;
  iso639_1?: string;
  iso639_2?: string;
  nativeName?: string;
}

export interface Pais {
  name: string;
  nativeName?: string;
  alpha3Code: string;
  region: string;
  languages: IdiomaPais[];
  flags?: { png?: string; svg?: string };
}
