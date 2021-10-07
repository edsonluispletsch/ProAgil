import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { DateTimeFormatPipePipe } from 'src/app/_helps/DateTimeFormatPipe.pipe';
import { Evento } from 'src/app/_models/Evento';
import { EventoService } from 'src/app/_services/evento.service';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.css'],
})
export class EventoEditComponent implements OnInit {
  titulo: string = 'Editar Evento';
  registerForm: FormGroup | any;
  evento: Evento = new Evento();
  imagemURL: string = 'assets/images/upload.png';
  file: File[] = [];
  imagemURLAnt: String = '';
  fileNameToUpdate: string = '';
  dataAtual: string = '';

  get lotes(): FormArray {
    return <FormArray>this.registerForm.get('lotes');
  }

  get redesSociais(): FormArray {
    return <FormArray>this.registerForm.get('redesSociais');
  }

  constructor(
    private eventoService: EventoService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService,
    private router: ActivatedRoute
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit() {
    this.validation();
    this.carregarEvento();
  }

  validation() {
    this.registerForm = this.fb.group({
      id: [''],
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      imagemURL: [''],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([]),
      redesSociais: this.fb.array([]),
    });
  }

  criaLote(lote: any): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }

  criaRedeSocial(redeSocial: any): FormGroup {
    return this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required],
    });
  }

  adicionarLote() {
    this.lotes.push(this.criaLote({ id: 0 }));
  }

  adicionarRedeSocial() {
    this.redesSociais.push(this.criaRedeSocial({ id: 0 }));
  }

  removerLote(id: number) {
    this.lotes.removeAt(id);
  }

  removerRedeSocial(id: number) {
    this.redesSociais.removeAt(id);
  }

  onFileChange(event: any) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      reader.onload = (event: any) => (this.imagemURL = event.target.result);

      this.file = event.target.files;
      reader.readAsDataURL(this.file[0]);
    }
  }

  carregarEvento() {
    const idEvento = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getEventoById(idEvento).subscribe((evento: Evento) => {
      this.evento = Object.assign({}, evento);
      this.fileNameToUpdate = evento.imagemURL?.toString();
      this.imagemURL = `http://localhost:5000/resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;
      this.evento.imagemURL = '';
      this.registerForm.patchValue(this.evento);

      this.evento.lotes.forEach((lote) => {
        this.lotes.push(this.criaLote(lote));
      });

      this.evento.redesSociais.forEach((redeSocial) => {
        this.redesSociais.push(this.criaRedeSocial(redeSocial));
      });
    });
  }

  salvarEvento() {
    this.evento = Object.assign(
      { id: this.evento.id },
      this.registerForm.value
    );
    this.uploadImagem();
    console.log(this.evento);
    this.eventoService.putEvento(this.evento).subscribe(
      () => {
        this.toastr.success('Editado com sucesso!');
      },
      (error) => {
        this.toastr.error(`Erro ao alterar registro: ${error}`);
      }
    );
  }

  uploadImagem() {
    const nomeArquivo = this.evento.imagemURL.split('\\', 3);
    this.evento.imagemURL = nomeArquivo[2] ?? this.fileNameToUpdate;
    if (this.registerForm.get('imagemURL').value !== '') {
      this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(() => {
        this.dataAtual = new Date().getMilliseconds().toString();
        this.imagemURL = `http://localhost:5000/resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;
      });
    }
  }

/*  onDateModelChange($event: NgbDateStruct | string, formControlName: string, datepickerInput: NgbInputDatepicker) {
		const requiredLength = 10;
		if (typeof $event !== 'string') return;
		if ($event.length !== requiredLength) return;
		if (!(formControlName in this.form.value)) throw new Error('Form control not found');


		const date: Date = this.StringToDateAdapter($event, 'dd/mm/yyyy'); // convert string value to date
		const ngbDate: NgbDateStruct = this.ngbDateAdapter.fromModel(date); // convert date to NgbDateStruct

		if (moment(date).isValid()) {
			this.form.patchValue({ [formControlName]: ngbDate }); // set value on form
			datepickerInput.dateSelect.emit(NgbDate.from(ngbDate)); // manually emit datepicker change
		}
	}*/

  StringToDateAdapter(date: string, format: string): Date {
    if (!date) return null;
  
    switch (format) {
      default:
        const dateString = (date || '').split('/').reverse().join('-');
        return moment(dateString).toDate();
    }
  }
}
