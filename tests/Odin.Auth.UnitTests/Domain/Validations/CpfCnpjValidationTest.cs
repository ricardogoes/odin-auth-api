using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Validations;

namespace Odin.Auth.UnitTests.Domain.Validations
{
    [Collection(nameof(CpfCnpjValidationTestFixture))]
    public class CpfCnpjValidationTest
    {
        private readonly CpfCnpjValidationTestFixture _fixture;

        public CpfCnpjValidationTest(CpfCnpjValidationTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "CpfCnpj() should validate when CNPJ is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void CNPJOk()
        {
            string fieldName = "Document";
            var cnpj = _fixture.GetValidCNPJ();
            Action action = () => CpfCnpjValidation.CpfCnpj(cnpj, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = "CpfCnpj() should throw an error when CNPJ is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void ThrowErrorWhenCNPJInvalid()
        {
            string? value = _fixture.GetInvalidCNPJ();
            string fieldName = "Document";

            Action action =
                () => CpfCnpjValidation.CpfCnpj(value, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "CpfCnpj() should validate when CPF is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void CPFOk()
        {
            string fieldName = "Document";
            var cpf = _fixture.GetValidCPF();
            Action action = () => CpfCnpjValidation.CpfCnpj(cpf, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = "CpfCnpj() should throw an error when CPF is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void ThrowErrorWhenCPFInvalid()
        {
            string? value = _fixture.GetInvalidCPF();
            string fieldName = "Document";

            Action action =
                () => CpfCnpjValidation.CpfCnpj(value, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "IsCpfCnpj() should return when CNPJ is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void IsCpfCnpj_CNPJ_Ok()
        {
            var cnpj = _fixture.GetValidCNPJ();            
            var validCnpj = CpfCnpjValidation.IsCpfCnpj(cnpj);
            validCnpj.Should().BeTrue();
        }

        [Fact(DisplayName = "IsCpfCnpj() should return when CNPJ is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void IsCpfCnpj_CNPJ_NOk()
        {
            string cnpj = _fixture.GetInvalidCNPJ();
            var validCnpj = CpfCnpjValidation.IsCpfCnpj(cnpj);
            validCnpj.Should().BeFalse();
        }

        [Fact(DisplayName = "IsCpfCnpj() should return when CPF is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void IsCpfCnpj_CPF_Ok()
        {
            var cpf = _fixture.GetValidCPF();
            var validCpf = CpfCnpjValidation.IsCpfCnpj(cpf);
            validCpf.Should().BeTrue();
        }

        [Fact(DisplayName = "IsCpfCnpj() should return when CPF is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void IsCpfCnpj_CPF_NOk()
        {
            string cpf = _fixture.GetInvalidCPF();
            var validCpf = CpfCnpjValidation.IsCpfCnpj(cpf);
            validCpf.Should().BeFalse();
        }
    }
}
