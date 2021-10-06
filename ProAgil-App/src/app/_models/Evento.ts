import { Lote } from './Lote';
import { Palestrante } from './Palestrante';
import { RedeSocial } from './RedeSocial';

export class Evento {

    constructor() {}

    id: number = 0;
    local: string = '';
    dataEvento: Date | any;
    tema: string = '';
    qtdPessoas: number = 0;
    imagemURL: string = '';
    telefone: string = '';
    email: string = '';
    lotes: Lote[] | any;
    redesSociais: RedeSocial[] | any;
    palestranteEvento: Palestrante[] | any;
}
